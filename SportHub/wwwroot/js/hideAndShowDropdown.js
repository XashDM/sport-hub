function openDropdownFunction(articleId) {
    document.getElementById(articleId.toString()).classList.toggle("show");
}

//window.onclick = function (event) {
//    if (!event.target.matches('.get-admins-articles-drop-btn')) {
//        var dropdowns = document.getElementsByClassName("get-admins-articles-dropdown-content");
//        var i;
//        for (i = 0; i < dropdowns.length; i++) {
//            var openDropdown = dropdowns[i];
//            if (openDropdown.classList.contains('show')) {
//                openDropdown.classList.remove('show');
//            }
//        }
//    }
//}