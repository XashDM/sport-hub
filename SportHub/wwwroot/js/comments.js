function submit_comment() {
	let comment = $('.commentar').val();
	let baseUrl = (window.location).href;
	let articleId = baseUrl.substring(baseUrl.lastIndexOf('=') + 1);
	let token = localStorage.getItem('Jwt Token');
	if (token) {
		token = JSON.parse(atob(token.split('.')[1]));
	}
	else {
		$('.box_comment').css('border', `#d72130 1px solid`);
		$('.commentar').val('You have to login in order to leave comments.').css('color', '#d72130');
    }
	el = document.createElement('li');
	fullName = document.getElementById("fullName").textContent = userData().name + ' ' + userData().family_name;
	var commentDate = new Date().toLocaleTimeString();
	$.ajax({
		headers:
		{
			'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
			"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: locationOrigin + '/api/Articles/CreateMainComment',
		type: 'post',
		data: JSON.stringify({
			"ArticleId": articleId,
			"Message": comment,
			"UserId": token.nameid
		}),
		success() {
			el.className = "box_result row";
			el.innerHTML =
				'<div class="comment-ex">' +
				'<div class="author-img">' +
				'<img class="userImg" src="https://static.xx.fbcdn.net/rsrc.php/v1/yi/r/odA9sNLrE86.jpg" alt="avatar" />' +
				'</div>' +
				'<div class="comment-info">' +
				'<div class="comment-author">' +
				'<h4 id="fullName">' + fullName + '</h4>' +
				'</div>' +
				'<div class="comment-date">' +
				'<span class="commentDate" id="commentDate">' + commentDate + '</span>' +
				'</div>' +
				'<div class="comment-text">' +
				'<p class="mainComment">' + comment + '</p>' +
				'</div>' +
				'<div class="tools_comment">' +
				//'<a class="like" href="#">Like</a>' +
				'<img class="like" src="/icons/like-icon.svg"  alt="Inactive"/>' +
				'<a class="replay" href="#">Comment</a>' +	
				'<span class="count">0</span>' +
				'<img class="dislike" src="/icons/dislike-icon.svg" alt="InactiveDislike"/>' +
				'<span class="countDislike">0</span>' +
				'<a href="#" class="showHide" data-id="myNewComment">' + ' ' +

				'<span class="delete-stuff pull-right">Delete &nbsp;</span>' +
				'<span class="edit-stuff pull-right">Edit &nbsp;</span>' +

				'</a >' +
				'</div>' +
				'</div >' +
				'</div>' +
				'<ul class="child_replay"></ul>'
				+ '</div>'
			document.getElementById('list_comment').prepend(el);
			$('.commentar').val('');
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

function cancel_comment() {
	$('.commentar').val('');
}

var locationOrigin;

$(document).ready(function () {
	locationOrigin = window.location.origin;
	let token = localStorage.getItem('Jwt Token');
	if (token) {
		token = JSON.parse(atob(token.split('.')[1]));
	};

	$('#list_comment').on('click', '.replay', function (e) {
		e.preventDefault();
		cancel_reply();
		$current = $(this);
		el = document.createElement('li');
		el.className = "box_reply row";
		el.innerHTML =
			'<div class=\"reply_comment\">' +
			'<div class=\"row\">' +
			'<div class=\"box_comment\">' +
			'<div class=\"avatar_comment\">' +
			'<img src=\"https://html5css.ru/howto/img_avatar2.png\" alt=\"avatar\"/>' +
			'<textarea id="textarea" class=\"comment_replay\" placeholder=\"Add a comment...\"></textarea>' +
			'</div>' +
			'<div class=\"box_post\">' +
			'<div class=\"pull-right\">' +
			'</span>' +
			'<button class=\"cancel\" onclick=\"cancel_reply()\" type=\"button\">Cancel</button>' +
			'<button onclick=\"submit_reply()\" type=\"button\" value=\"1\">Comment</button>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>';
		$current.closest('li').find('.child_replay').prepend(el);
	});

	$('#list_comment').on('click', '.delete-stuff', function (e) {
		e.preventDefault();
		const id = (e.target.parentElement.dataset).id;
		$.ajax({
			headers:
			{
				'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
				"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
			},
			async: false,
			url: locationOrigin + '/api/Articles/DeleteComment/?mainCommentId=' + id,
			type: 'delete',
			success: function () {
				$(e.target.parentElement.parentElement.parentElement.parentElement.parentElement).remove();
			},
			error(errorThrown) {
				console.log(errorThrown);
			}
		});
	});
	var counter = 0;
	var lastEditedComment = null;
	$('#list_comment').on('click', '.edit-stuff', function (e) {
		e.preventDefault();
		counter++;
		const id = (e.target.parentElement.dataset).id;
		let currentCommentElement = $(this).closest('.comment-ex');
		var text = $(this).closest('.comment-info').find('.mainComment').text();

		if (!lastEditedComment) {
			lastEditedComment = [currentCommentElement, text];
		}
		else {
			const commentElement = lastEditedComment[0];
			const commentText = lastEditedComment[1];

			commentElement.find('.mainComment').val(commentText);
		}
		if (counter === 1) {
			$(this).closest('.comment-info').find('.comment-text').replaceWith("<textarea class=\"editedComment\">" + text + "</textarea >");
			$(e.target.parentElement.parentElement).append("<div class=\"box_post\">" +
				"<div class=\"pull-right\">" +
				"<button id=\"cancelBtn\">Cancel</button>" +
				"<button onclick=\"submit_edit(" + id + ")\">Submit</button>" +
				"</div>" +
				"</div>");
		}
		else {
			$('.box_post').eq(1).remove();
			$('.editedComment').replaceWith("<div class=\"comment-text\"><p class=\"mainComment\">" + lastEditedComment[1] + "</p></div>");
			$(this).closest('.comment-info').find('.comment-text').replaceWith("<textarea class=\"editedComment\">" + text + "</textarea >");
			$(e.target.parentElement.parentElement).append("<div class=\"box_post\">" +
				"<div class=\"pull-right\">" +
				"<button id=\"cancelBtn\"\">Cancel</button>" +
				"<button id=\"submitBtn\" onclick=\"submit_edit(" + id + ")\">Submit</button>" +
				"</div>" +
				"</div>");

			lastEditedComment = [currentCommentElement, text];
		}

	});

	$('#list_comment').on('click', '#cancelBtn', function (e) {
		e.preventDefault();
		let text = $(this).closest('.comment-info').find('.editedComment').text();
		$('.box_post').eq(1).remove();
		$('.editedComment').replaceWith("<div class=\"comment-text\"><p class=\"mainComment\">" + text + "</p></div>");
	});
	
	$('#list_comment').on('click', '.like', function (e) {
		e.preventDefault();
		const id = (e.target.parentElement.dataset).id
		sendLikeOrDislike(id, token.nameid, true, $(this));
	});

	$('#list_comment').on('click', '.dislike', function (e) {
		e.preventDefault();
		const id = (e.target.parentElement.dataset).id;
		sendLikeOrDislike(id, token.nameid, false, $(this));
	});

	const baseUrl = (window.location).href;
	const articleId = baseUrl.substring(baseUrl.lastIndexOf('=') + 1);

	$('#selectSort').on('change', function (e) {
		var selected = $(this).val();
		$('#list_comment').empty();
		getSortedComments(selected, articleId, generatePageArguments(1, 5));
	});

	getCommentCount(articleId);
	getSortedComments("newest", articleId, generatePageArguments(1, 5));
});
let liked = 0;
let disliked = 0;
function sendLikeOrDislike(mainCommentId, userId, isLiked, $current) {
	$.ajax({
		headers:
		{
			'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: locationOrigin + '/api/Articles/LikeOrDislikeComment',
		data: JSON.stringify({
			"MainCommentId": mainCommentId,
			"UserId": userId,
			"IsLiked": isLiked
		}),
		type: 'post',
		success: function () {
			if (isLiked) {
				var x = $current.closest('div').find('.like').toggleClass('red-accent-img');
				var y = parseInt($current.closest('div').find('.count').text().trim());
				$current.closest('div').find('.count').text(y + 1);
				//if (liked%2==0) {
				//	$current.closest('div').find('.count').text(y + 1);
				//	liked += 1;
				//}
				//else {
				//	$current.closest('div').find('.count').text(y - 1);
				//	liked += 1;
				//}
			}
			else {
				var x = $current.closest('div').find('.dislike').toggleClass('red-accent-img');
				var y = parseInt($current.closest('div').find('.countDislike').text().trim());
				$current.closest('div').find('.countDislike').text(y + 1);
				//if (disliked % 2 == 0) {
				//	$current.closest('div').find('.countDislike').text(y + 1);
				//	disliked += 1;
				//}
				//else {
				//	$current.closest('div').find('.countDislike').text(y - 1);
				//	disliked += 1;
				//}
            }
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

function generatePageArguments(pageNumber, pageSize) {
	const pageArguments = { 'PageNumber': pageNumber, 'PageSize': pageSize }

	return pageArguments;
}

function getSortedComments(type, articleId, pageArgs) {
	$.ajax({
		headers:
		{
			"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		data: JSON.stringify({
			"PageArgs": pageArgs,
			"SortedBy": type,
			"ArticleId": articleId
		}),
		url: locationOrigin + '/api/Articles/GetSortedComments',
		type: 'post',
		success: function (data) {
			console.log(data);
			console.log("Ігор киця");
			$('#list_comment').empty();
			displayAllComments(data);
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

function submit_edit(id) {
	var comment = $('.editedComment').val();
	$.ajax({
		headers:
		{
			'Authorization': 'Bearer ' + localStorage.getItem('Jwt Token'),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: locationOrigin + '/api/Articles/EditComment',
		type: 'put',
		data: JSON.stringify({
			"MainCommentId": id,
			"Message": comment
		}),
		success() {
			location.reload();
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

function displayAllComments(data) {
	let token = localStorage.getItem('Jwt Token');
	if (token) {
		parsedToken = JSON.parse(atob(token.split('.')[1]));
	}
	else {
		for (let i = 0; i <= data.length; i++) {
			el = document.createElement('li');
			el.className = "box_result row";
			el.innerHTML =
				'<div class="comment-ex">' +
				'<div class="author-img">' +
				'<img class="userImg" src="https://static.xx.fbcdn.net/rsrc.php/v1/yi/r/odA9sNLrE86.jpg" alt="avatar" />' +
				'</div>' +
				'<div class="comment-info">' +
				'<div class="comment-author">' +
				'<h4 id="fullName">' + data[i].user.firstName + ' ' + data[i].user.lastName + '</h4>' +
				'</div>' +
				'<div class="comment-date">' +
			'<span class="commentDate" id="commentDate">' + formatDateMy(data[i].created) + '</span>' +
				'</div>' +
				'<div class="comment-text">' +
				'<p class="mainComment">' + data[i].message + '</p>' +
				'</div>' +
				'<div class="tools_comment" data-id=' + data[i].id + '>' +
			//'<a class="like" href="#">Like</a>' +
				'<img class="like" src="/icons/like-icon.svg" alt="Inactive"/>' +

				/*'<a class="replay" href="#">Comment</a>' +*/
				'<i class="fa fa-thumbs-o-up"></i> <span class="count">' + data[i].likes + '</span>' +
				'<img class="dislike" src="/icons/dislike-icon.svg" alt="InactiveDislike"/>' +
				'<span class="countDislike">' + data[i].dislikes + '</span>' +
				'<a href="#" class="showHide" data-id=' + data[i].id + '>' + ' ' +
				'</a >' +
				'</div>' +
				'</div >' +
				'</div>' +
				'<ul class="child_replay"></ul>'
				+ '</div>'
			document.getElementById('list_comment').append(el);
			$('.commentar').val('');
		}
    }
	for (let i = 0; i <= data.length; i++) {
		el = document.createElement('li');
		el.className = "box_result row";
		var equal = data[i].userId == parsedToken.nameid;
		if (equal) {
			el.innerHTML = '<div class="comment-ex">' +
				'<div class="author-img">' +
				'<img class="userImg" src="https://static.xx.fbcdn.net/rsrc.php/v1/yi/r/odA9sNLrE86.jpg" alt="avatar" />' +
				'</div>' +
				'<div class="comment-info">' +
				'<div class="comment-author">' +
				'<h4 id="fullName">' + data[i].user.firstName + ' ' + data[i].user.lastName + '</h4>' +
				'</div>' +
				'<div class="comment-date">' +
				'<span class="commentDate" id="commentDate">' + formatDateMy(data[i].created) + '</span>' +
				'</div>' +
				'<div class="comment-text">' +
				'<p class="mainComment">' + data[i].message + '</p>' +
				'</div>' +
				'<div class="tools_comment" data-id=' + data[i].id + '>' +
				//'<a class="like" href="#">Like</a>' +
				'<img class="like" src="/icons/like-icon.svg" alt="Inactive"/>' +
				'<a class="replay" href="#">Comment</a>' +
				'<i class="fa fa-thumbs-o-up"></i> <span class="count">' + data[i].likes + '</span>' +
				'<img class="dislike" src="/icons/dislike-icon.svg" alt="InactiveDislike"/>' +
				'<span class="countDislike">' + data[i].dislikes + '</span>' +
				'<a href="#" class="showHide" data-id=' + data[i].id + '>' + ' ' +

				'<span class="delete-stuff pull-right">Delete &nbsp;</span>' +
				'<span class="edit-stuff pull-right">Edit &nbsp;</span>' +

				'</a >' +
				'</div>' +
				'</div >' +
				'</div>' +
				'<ul class="child_replay"></ul>'
				+ '</div>'
		}
		else {
			el.innerHTML =
				'<div class="comment-ex">' +
				'<div class="author-img">' +
				'<img class="userImg" src="https://static.xx.fbcdn.net/rsrc.php/v1/yi/r/odA9sNLrE86.jpg" alt="avatar" />' +
				'</div>' +
				'<div class="comment-info">' +
				'<div class="comment-author">' +
				'<h4 id="fullName">' + data[i].user.firstName + ' ' + data[i].user.lastName + '</h4>' +
				'</div>' +
				'<div class="comment-date">' +
			'<span class="commentDate" id="commentDate">' + formatDateMy(data[i].created) + '</span>' +
				'</div>' +
				'<div class="comment-text">' +
				'<p class="mainComment">' + data[i].message + '</p>' +
				'</div>' +
			'<div class="tools_comment" data-id=' + data[i].id + '>' +
			//'<a class="like" href="#">Like</a>' +
			'<img class="like" src="/icons/like-icon.svg" alt="Inactive"/>' +
				'<a class="replay" href="#">Comment</a>' +
				'<i class="fa fa-thumbs-o-up"></i> <span class="count">' + data[i].likes + '</span>' +
				'<img class="dislike" src="/icons/dislike-icon.svg" alt="InactiveDislike"/>' +
				'<span class="countDislike">' + data[i].dislikes + '</span>' +
				'<a href="#" class="showHide" data-id=' + data[i].id + '>' + ' ' +

				//'<span class="delete-stuff pull-right">Delete &nbsp;</span>' +
				//'<span class="edit-stuff pull-right">Edit &nbsp;</span>' +

				'</a >' +
				'</div>' +
				'</div >' +
				'</div>' +
				'<ul class="child_replay"></ul>'
				+ '</div>'
		}
		document.getElementById('list_comment').append(el);
		$('.commentar').val('');
	}
}

function formatDateMy(str) {
	const indexOfT = 10;
	const indexOfSeconds = 16;
	let datee = str.substring(0, indexOfT);
	let timee = str.substring(indexOfT + 1, indexOfSeconds);
	let splited = datee.split("-");
	let normalDate = timee + ' ' + splited[2] + '.' + splited[1] + '.' + splited[0];
	return normalDate;
}

function getCommentCount(articleId) {	
	$.ajax({
		headers:
		{
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: locationOrigin + '/api/Articles/GetCommentsCount?articleId=' + articleId,
		type: 'get',
		success: function (data) {
			$('.count_comment').text('COMMENTS (' + data + ')');
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

