@page "/page"
@model SportHub.Pages.Admin.ManageNavigationItems.IndexModel
@{
    ViewData["Title"] = "Manage";
}

<div id="screen">
    <div class='contener-for-adding' style="display: none" id='formid'>

        <form class="form-to-add" id='form-for-adding'>
            <div class='main-content'>
                <span class="text-lable" id='labe-with-description'>Add</span>
                <label for="Name"><b>Name</b></label>
                <input type="text" placeholder="Name of your menu item" name="Name" required id='item-name-input'>
                    <div class='addbutton' id='add-button-id'>Add</div>
                    <div onclick="document.getElementById('formid').style.display='none'" class="cancelbtn">Cancel</div>
        </div>
    </form>

  </div>
        <div class='Category-Contener' id='Category-Contener'>

            <ul class='Category' id='Category'>
            </ul>
        </div>
    </div>
    @section scripts {
        <script type="text/javascript">
            const ContenerforNestedType = {
                Category: "Subcategory-Contener",
            Subcategory: "Team-Contener",
            Team: null,
};
            let activeItem = {
                Category: null,
            Subcategory: null,
            Team: null,
};
            let CategoryFDateList = null;
            let SubcategoryDateList = null;
            let TeamDateList = null;

            let EctiveCategory = "";
            let EctiveSubCategory = "";
            let lastEctiveItem = null;

            //Get list of Teams from server by ID of their father 
            function getTeamofSubcategory() {
                let result;

            obj = fatherfor('Team');
            if (obj == null) {
        return [];
    }
            $.ajax({
                url: '@Url.Page("Index","Children")',
            type: "GET",
            data: {ItemId: obj.id },
            async: false,
            headers: {
                RequestVerificationToken: $(
            'input:hidden[name="__RequestVerificationToken"]'
            ).val(),
        },
    }).done(function (date) {
                result = date;
    });
            return result;
}
            //Get list of Subcategory from server
            function getSubcategoryofCategory() {
                let result;

            obj = fatherfor["Category"];
            if (obj == null) {
        return [];
    }
            $.ajax({
                url: '@Url.Page("Index","Children")',
            type: "GET",
            data: {ItemId: obj.id },
            async: false,
            headers: {
                RequestVerificationToken: $(
            'input:hidden[name="__RequestVerificationToken"]'
            ).val(),
        },
    }).done(function (date) {
                result = date;
    });

            return result;
}
            //Get list of Categorys from server
            function getCategory() {
                let result;
            $.ajax({
                url: '@Url.Page("Index","Root")',
            data: $("#tab"),
            async: false,
    }).done(function (date) {
                result = date;
    });

            return result;
}
            // When click on item this functon is called by that item and get parameter his info - 'obj'
            function getContenerforType(element) {
    return ContenerforNestedType[element];
}
            function removeChildrenFromVisualTree(elementForDelete) {
                let delet = document.getElementsByClassName(elementForDelete);
    while (delet.length > 0) delet[0].remove();
}
            function openTreeforItem(obj) {
                let tipeOfButton;
            let elementForDelete;
            if(obj == null){
                createCategory();
            let list =getElementsById(Category);
            list.innerHTML = "";
    }
            else{
                tipeOfButton = obj.type;

            elementForDelete = getContenerforType(obj.type);

            if (elementForDelete == null) {
        return 0;
        }
    }

            removeChildrenFromVisualTree(elementForDelete)

            if (tipeOfButton == "Category") {
                activeItem[tipeOfButton] = obj;
            lastEctiveItem = obj;
            createSubcategory();
    } else if (tipeOfButton == "Subcategory") {
                activeItem[tipeOfButton] = obj;
            lastEctiveItem = obj;
            createTeam();
    }
}
            //Get father element for item 'type' - [Category, Subcategory, Team]
            function fatherfor(type) {
    if (type == "Category") {
        return null;
    } else if (type == "Subcategory") {
        return activeItem["Category"];
    } else if (type == "Team") {
        return activeItem["Subcategory"];
    }
}
            //Get father ID element for item 'type' - [Category, Subcategory, Team]
            function fatherIdfor(type) {
    if (type == "Category") {
        return null;
    } else if (type == "Subcategory") {
        return activeItem["Category"].id;
    } else if (type == "Team") {
        return activeItem["Subcategory"].id;
    }
}
            // function for opening Form called from add_button 
            function openForm(type) {
                let label = document.getElementById("labe-with-description");
            label.innerHTML = "Add new " + type.toLowerCase();
            document.getElementById("formid").style = "display: fixed";
            let button = document.getElementById("add-button-id");
            button.onclick = function (e) {
                createItem(type);
            e.stopPropagation();
    };
}
            // function for adding new item and push to server called by add-button on form
            function createItem(type) {
                let name = document.getElementById("item-name-input").value;
            let fatherItemId = fatherIdfor(type);
            $.ajax({
                url: '@Url.Page("Index","AddItem")',
            data: {
                Type: type,
            Name: name,
            FatherItemId: fatherItemId,
        },
    }).done(function (date) {
                openTreeforItem(fatherfor(type));
    });
            document.getElementById("formid").style.display = "none";
}
            //function for adding Side line on item list for tree view
            function createSideLine(type, SizeOfList) {
                let line = document.createElement("div");
            line.setAttribute("class", "SideLine");
            line.setAttribute("id", "SideLine-" + type);
            line.style.height = 60 * (SizeOfList - 1) + "px";
            return line;
}
            ///function for adding Button which open form. Called by all function to create Category/...
            function createButton(type) {
                let button = document.createElement("div");
            button.setAttribute("class", "button-open-form");
            button.setAttribute("id", "button-open-form-" + type);
            button.innerHTML = "+Add " + type;
            button.onclick = function (e) {
                openForm(type);
            e.stopPropagation();
    };
            return button;
}
            //Create list of elemnt form 'Date' - array , ItamClass -css class for element  'ItemId' css id for element 
            function сreateList(date, itamClass, itemId, listElement) {
                let sizeOfList = 0;
            for (let e in date) {
                let element = date[e];
            let name = element.name;
            let categoryListElement = document.createElement("li");
            categoryListElement.setAttribute("class", itamClass);
            categoryListElement.setAttribute("id", itemId + "-" + name);
            listElement.appendChild(categoryListElement);

            categoryListElement.onclick = function (e) {
                openTreeforItem(element);
            e.stopPropagation();
        };

            categoryListElement.innerHTML = name;
            sizeOfList += 1;
    }
            return sizeOfList;
}
            //Create Categorys list on screen 
            function createCategory() {
                let type = "Category";
            let categoryDate = getCategory();
            let nameofCategory = "Category";


            let categoryContener = document.getElementById("Category-Contener");

            let categoryContener = document.createElement("div");
            subcategoryContener.setAttribute("class", "Category-Contener");
            subcategoryContener.setAttribute("id", "div-" + nameofSubcategory);

            let categoryList = document.getElementById("Category");

            categoryContener.prepend(createButton(type));

            сreateList(categoryDate, "item", nameofCategory, categoryList);
}
            //Create Subcategorys list on screen 
            function createSubcategory() {
                let type = "Subcategory";
            let categoryName = activeItem["Category"].name;
            let nameofSubcategory = categoryName + "-Subcategory";
            let subcategoryDate = getSubcategoryofCategory();

            let subcategory = document.getElementById("Category-" + categoryName);

            let subcategoryContener = document.createElement("div");
            subcategoryContener.setAttribute("class", "Subcategory-Contener");
            subcategoryContener.setAttribute("id", "div-" + nameofSubcategory);
            subcategory.appendChild(subcategoryContener);

            subcategoryContener.prepend(createButton(type));

            let subcategoryList = document.createElement("ul");
            subcategoryList.setAttribute("id", nameofSubcategory);
            subcategoryList.setAttribute("class", type);
            subcategoryContener.appendChild(subcategoryList);

            let sizeOfList = сreateList(
            subcategoryDate,
            "item other",
            type,
            subcategoryList
            );

            subcategoryContener.append(createSideLine(type, sizeOfList));
}
            //Create Teams list on screen 
            function createTeam() {
                let type = "Team";
            let subcategoryName = activeItem["Subcategory"].name;
            let teamDate = getTeamofSubcategory();
            let namaofTeaminSubcategory = subcategoryName + "-Team";
            let fullName = "Subcategory" + "-" + subcategoryName;

            let team = document.getElementById(fullName);

            let teamContener = document.createElement("div");
            teamContener.setAttribute("class", "Team-Contener");
            teamContener.setAttribute("id", "div-" + namaofTeaminSubcategory);
            team.appendChild(teamContener);

            teamContener.prepend(createButton(type));

            let teamList = document.createElement("ul");
            teamList.setAttribute("id", namaofTeaminSubcategory);
            teamList.setAttribute("class", "Team");
            teamContener.appendChild(teamList);

            let SizeOfList = сreateList(
            teamDate,
            "item other",
            namaofTeaminSubcategory,
            teamList
            );

            teamContener.append(createSideLine(type, SizeOfList));
}

            createCategory();
            var modal = document.getElementById("formid");

            window.onclick = function (event) {
    if (event.target == modal) {
                modal.style.display = "none";
    }
};
        </script>
    }