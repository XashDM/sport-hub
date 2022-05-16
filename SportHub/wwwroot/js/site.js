let timer1;
let timer2;
let DateGetClass = new ExtendDateClass();
$("#team-side, #subcategory-sidebar").mouseleave(function () {
    timer1 = setTimeout(HideTeam, 10);
}).mouseenter(function () {
    clearTimeout(timer1);
});
$("#category-sidebar, #subcategory-side, #team-side").mouseleave(function () {
    timer2 = setTimeout(HideSubcategory, 10);
}).mouseenter(function () {
    clearTimeout(timer2);
});
function HideTeam() {
    console.log('Team left');
    $("#team-side").css({
        display: "none"
    });
}
function HideSubcategory() {
    console.log('Subcategory left');
    $("#subcategory-side").css({
        display: "none"
    });
    $("#blureid").css({
        display: "none"
    });
}
function CreateCatgorySidebar() {
    let List = $("#category-sidebar")
    List.empty();
    List.css({
        display: "block"
    })
    let date = DateGetClass.getCategory();
    console.log(date);
    for (let e in date) {
        let element = date[e];
        var li = $(`<li/>`)
            .text(element.name)
            .mouseenter(function () {
                CreateSubcatgorySidebar(element);
            })
            .appendTo(List);
    }
}
function CreateSubcatgorySidebar(e) {
    let List = $("#subcategory-sidebar");
    List.empty();
    $("#subcategory-side").css({
        display: "block"
    });
    $("#blureid").css({
        display: "block"
    });
    let date = DateGetClass.getSubcategoryofCategory(e);
    for (let e in date) {
        let element = date[e];
        var li = $(`<li/>`)
            .text(element.name)
            .addClass('item-sidebar')
            .mouseenter(function () {
                CreateTeamSidebar(element);
            })
            .appendTo(List);
    }
}
function CreateTeamSidebar(e) {
    let List = $("#team-sidebar");
    List.empty();
    $("#team-side").css({
        display: "block"
    })
    $("#blureid").css({
        display: "block"
    });
    let date = DateGetClass.getTeamofSubcategory(e);
    for (let e in date) {
        let element = date[e];
        var li = $(`<li/>`)
            .text(element.name)
            .addClass('item-sidebar')
            .appendTo(List);
    }
}
CreateCatgorySidebar();