﻿let NIDateClass = new DateClass();

const ContenerforNestedType = {
    Category: "Subcategory-Contener",
    Subcategory: "Team-Contener",
    Team: null,
    global: "Category-Contener"
};
let activeItem = {
    Category: null,
    Subcategory: null,
    Team: null,
};
let CategoryDateList = null;
let SubcategoryDateList = {};
let TeamDateList = {};
let Tree = null;
let EctiveCategory = "";
let EctiveSubCategory = "";
let lastActiveItem = null;
let SaveInMoment = false;
let addedItem = [];
//Get list of Teams from server by ID of their father 

function parantFor(type) {
    if (type == "Category") {
        return null;
    } else if (type == "Subcategory") {
        return activeItem["Category"];
    } else if (type == "Team") {
        return activeItem["Subcategory"];
    }
}
function getTeamofSubcategory() {
    let result;

    parent = fatherfor('Team');
    if (parent == null) {
        return [];
    }

    console.log(parent);
    console.log(TeamDateList);
    if (parent.children == null) {
        let parentsId = parent.id;
        $.ajax({
            url: '/GetChildren',
            type: "GET",
            data: { ItemId: parentsId },
            async: false,
            headers: {
                RequestVerificationToken: $(
                    'input:hidden[name="__RequestVerificationToken"]'
                ).val(),
            },
        }).done(function (date) {
            parent.children = date;
            result = date;
        });
    }
    else {
        result = parent.children
    }
    console.log(Tree);
    return result;
}
//Get list of Subcategory from server
function getSubcategoryofCategory() {
    let result;
    parent = fatherfor("Subcategory");
    if (parent == null) {
        return [];
    }
    console.log(parent);
    console.log(SubcategoryDateList);

    if (parent.children == null) {
        let parentsId = parent.id;
        $.ajax({
            url: '/GetChildren',
            type: "GET",
            data: { ItemId: parentsId },
            async: false,
            headers: {
                RequestVerificationToken: $(
                    'input:hidden[name="__RequestVerificationToken"]'
                ).val(),
            },
        }).done(function (date) {

            parent.children = date;
            result = date;
        });
    }
    else {
        result = parent.children
    }
    console.log(Tree);
    return result;
}function getSubcategoryofCategory() {
    let result;
    parent = fatherfor("Subcategory");
    if (parent == null) {
        return [];
    }
    console.log(parent);
    console.log(SubcategoryDateList);

    if (parent.children == null) {
        let parentsId = parent.id;
        $.ajax({
            url: '/GetChildren',
            type: "GET",
            data: { ItemId: parentsId },
            async: false,
            headers: {
                RequestVerificationToken: $(
                    'input:hidden[name="__RequestVerificationToken"]'
                ).val(),
            },
        }).done(function (date) {

            parent.children = date;
            result = date;
        });
    }
    else {
        result = parent.children
    }
    console.log(Tree);
    return result;
}
//Get list of Categorys from server
function getCategory() {
    let result;
    if (Tree == null) {
        $.ajax({
            url: '/GetRoot',
            data: $("#tab"),
            async: false,
        }).done(function (date) {
            result = date;
            Tree = date;
        });
    }
    else {
        result = Tree
    }
    console.log(Tree);
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
    let typeOfButton;
    if (obj == null) {
        typeOfButton = "global";
    }
    else {
        typeOfButton = obj.type;
    }


    let elementForDelete = getContenerforType(typeOfButton);

    if (elementForDelete == null) {
        return 0;
    }
    removeChildrenFromVisualTree(elementForDelete)
    console.log(typeOfButton);
    console.log(obj);
    if (typeOfButton == "Category") {
        activeItem[typeOfButton] = obj;
        lastActiveItem = obj;
        createSubcategory();
    } else if (typeOfButton == "Subcategory") {
        activeItem[typeOfButton] = obj;
        lastActiveItem = obj;
        createTeam();
    } else {
        createCategory();
    }
}
//Get father element for item 'type' - [Category, Subcategory, Team]
function fatherfor(type) {
    console.log(type);
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
function saveItem() {
    $.ajax({
        url: '/save',
        type: 'post',
        dataType: "json",
        data: JSON.stringify({
            date: addedItem
        }),//$.param({ data: addedItem}, true),//JSON.stringify({ addedItem }), // addedItem,
        async: false,
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
    }).done(function (date) {
        openTreeforItem(fatherfor(type));
    });
}
// function for adding new item and push to server called by add-button on form
function createItem(type) {
    let name = document.getElementById("item-name-input").value;
    let ParentsItemId = fatherIdfor(type);
    let item = {
        type: type,
        name: name,
        parentsItemId: ParentsItemId,
        children: []
    };
    if (SaveInMoment) {
        $.ajax({
            url: '@Url.Page("Index","AddItem")',
            data: item,
        }).done(function (date) {
            openTreeforItem(fatherfor(type));
        });
    }
    else {
        if (type == "Category") {
            Tree.push(item);
            openTreeforItem(null);
            addedItem.push(item);
            console.log("addedItem");
            console.log(addedItem);
        }
        else {
            parent = fatherfor(type);
            parent.children.push(item);
            if (item.parentsItemId != null) {
                addedItem.push(item);
                console.log("addedItem");
                console.log(addedItem);
            }
            openTreeforItem(parent);
        }
    }
    document.getElementById("formid").style.display = "none";
}
//function for adding Side line on item list for tree view
function createSideLine(type, SizeOfList) {
    let line = document.createElement("div");
    line.setAttribute("class", "SideLine");
    line.setAttribute("id", "SideLine-" + type);
    line.style.height = 60.6 * (SizeOfList - 1) + "px";
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
    console.log(itamClass);
    console.log(date);
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


    let screan = document.getElementById("screen");

    let categoryContener = document.createElement("div");
    categoryContener.setAttribute("class", "Category-Contener");
    categoryContener.setAttribute("id", "div-Category-Contener");
    screan.appendChild(categoryContener);

    let categoryList = document.createElement("ul");
    categoryList.setAttribute("id", type);
    categoryList.setAttribute("class", type);
    categoryContener.appendChild(categoryList);

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
let B = document.getElementById("addId");
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
};