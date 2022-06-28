function submit_comment(){
  var comment = $('.commentar').val();
	el = document.createElement('li');
	fullName = document.getElementById("fullName").textContent = userData().name + ' ' + userData().family_name;
	commentDate = new Date();
	$.ajax({
		headers:
		{
			"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: 'api/Articles/CreateMainComment',
		type: 'post',
		data: JSON.stringify({
			"ArticleId": 2,
			"Message": comment,
			"UserId": 1
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
				'<span class="commentDate" id="commentDate">' + formatDate(commentDate) + '</span>' +
				'</div>' +
				'<div class="comment-text">' +
				'<p class="mainComment">' + comment + '</p>' +
				'</div>' +
				'<div class="tools_comment">' +
				'<a class="like" href="#">Like</a>' +
				'<a class="replay" href="#">Comment</a>' +
				'<i class="fa fa-thumbs-o-up"></i> <span class="count">5</span>' +
				'<a class="dislike" href="#">Dislike</a>' +
				'<i class="fa fa-thumbs-o-up"></i> <span class="countDislike">0</span>' +
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

$(document).ready(function() {

	$('#list_comment').on('click', '.replay', function (e) {
		cancel_reply();
		$current = $(this);
		el = document.createElement('li');
		el.className = "box_reply row";
		el.innerHTML =
			'<div class=\"reply_comment\">'+
				'<div class=\"row\">'+
					'<div class=\"box_comment\">' +
						'<div class=\"avatar_comment\">' +
							'<img src=\"https://html5css.ru/howto/img_avatar2.png\" alt=\"avatar\"/>' +
							'<textarea id="textarea" class=\"comment_replay\" placeholder=\"Add a comment...\"></textarea>' +
						'</div>'+
					  '<div class=\"box_post\">'+
						'<div class=\"pull-right\">'+						
						  '</span>'+
						  '<button class=\"cancel\" onclick=\"cancel_reply()\" type=\"button\">Cancel</button>'+
						  '<button onclick=\"submit_reply()\" type=\"button\" value=\"1\">Comment</button>'+
						'</div>'+
					  '</div>'+
					'</div>'+
				'</div>'+
			'</div>';
		$current.closest('li').find('.child_replay').prepend(el);
	});

	$('#list_comment').on('click', '.delete-stuff', function (e) {
		const id = (e.target.parentElement.dataset).id;
		console.log(id);
		$.ajax({
			headers:
			{
				"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
			},
			async: false,
			url: `api/Articles/DeleteComment/?mainCommentId=${id}`,
			type: 'delete',
			success: function () {
				$(e.target.parentElement.parentElement.parentElement.parentElement.parentElement).remove();
			},
			error(errorThrown) {
				console.log(errorThrown);
			}
		});
	});

	getComments();
});

function getComments() {
	$.ajax({
		headers:
		{
			"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: 'api/Articles/GetComments?articleId=2',
		type: 'get',
		success: function (data) {
			displayAllComments(data);
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

function sortComments(type) {
	console.log(type);
	$.ajax({
		headers:
		{
			"RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		async: false,
		url: 'api/Articles/GetSortedComments/?sortedBy=' + type,
		type: 'get',
		success: function (data) {
			document.getElementById('list_comment').empty();
			displayAllComments(data);
		},
		error(errorThrown) {
			console.log(errorThrown);
		}
	});
}

function submit_reply(){
  var comment_replay = $('.comment_replay').val();
  el = document.createElement('li');
  el.className = "box_reply row";
	el.innerHTML =
		'<div class="comment-ex">'+
		'<div class="author-img">' +
			'<img class="userImg" src="https://static.xx.fbcdn.net/rsrc.php/v1/yi/r/odA9sNLrE86.jpg" alt="avatar" />' +
		'</div>' +
		'<div class="comment-info">' +
			'<div class="comment-author">' +
				'<h4>Aythorrrr</h4>' +
			'</div>' +
			'<div class="comment-date">' +
				'<span>26m</span>' +
			'</div>' +
			'<div class="comment-text">' +
				'<p>'+comment_replay+'</p>' +
				
			'</div>' +
		'<div class="tools_comment">' +
			'<a class="like" href="#">Like</a>' +
			'<a class="replay" href="#">Comment</a>' +
		'<i class="fa fa-thumbs-o-up"></i> <span class="count">0</span>' +
		'<a class="dislike" href="#">Dislike</a>' +
		'<i class="fa fa-thumbs-o-up"></i> <span class="countDislike">0</span>' +
		'</div>' +
		'<ul class="child_replay"></ul>'+
		'</div>'
	'</div>';
	$current.closest('li').find('.child_replay').prepend(el);
	$('.comment_replay').val('');
	cancel_reply();
}

function cancel_reply(){
	$('.reply_comment').remove();
}

function padTo2Digits(num) {
	return num.toString().padStart(2, '0');
}

function formatDate(date) {
	return [
		padTo2Digits(date.getDate()),
		padTo2Digits(date.getMonth() + 1),
		date.getFullYear(),
	].join('/');
}

function displayAllComments(data) {
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
			'<span class="commentDate" id="commentDate">' + data[i].created + '</span>' +
			'</div>' +
			'<div class="comment-text">' +
			'<p class="mainComment">' + data[i].message + '</p>' +
			'</div>' +
			'<div class="tools_comment">' +
			'<a class="like" href="#">Like</a>' +
			'<a class="replay" href="#">Comment</a>' +
			'<i class="fa fa-thumbs-o-up"></i> <span class="count">5</span>' +
			'<a class="dislike" href="#">Dislike</a>' +
			'<a href="#" data-id='+ data[i].id + '>' + ' ' +
                      '<span class="delete-stuff">Delete</span>'+
            '</a >'+
			'<i class="fa fa-thumbs-o-up"></i> <span class="countDislike">0</span>' +
			'</div>' +
			'</div >' +
			'</div>' +
			'<ul class="child_replay"></ul>'
			+ '</div>'
		document.getElementById('list_comment').prepend(el);
		$('.commentar').val('');
    }
}
