const  h={
      'Category':'Subcategory-Contener',
      'Subcategory':'Team-Contener',
      'Team': null
    };
    let activeItem = {
      'Category': null,
      'Subcategory':null,
      'Team': null
    }
   let CategoryFDateList= null;
    let SubcategoryDateList= null;
   let TeamDateList= null;

let EctiveCategory = '';
let EctiveSubCategory = '';
let lastEctiveItem = null;
function getTeamofSubcategory(){
    let  result;
    obj=activeItem['Subcategory'];
        console.log(obj);
    if(obj == null){
        return [];
    }
    $.ajax({
          url: '@Url.Page("Index","Children")',
          type: "GET",
          data: {"ItemId":obj.id},
          async: false,
          headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            }
       }).done(function(date) {
          console.log(date);
          result = date;
       });
    return result;
}
function getSubcategoryofCategory(){
    let  result;
    obj=activeItem['Category'];
        console.log(obj);
    if(obj == null){
        return [];
    }
    $.ajax({
          url: '@Url.Page("Index","Children")',
          type: "GET",
          data: {"ItemId":obj.id},
          async: false,
          headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
          }
       }).done(function(date) {
          console.log(date);
          result = date;
       });

   return result;
}
function getCategory(){ 
    let  result;
    $.ajax({
          url: '@Url.Page("Index","Root")',
          data : $("#tab"),
          async: false
    }).done(function(date) {
        console.log(date);
        result = date;
    });
    return result;
}

function pars(id){
  var partsArray = id.split('-');
  return partsArray
}
function getContenerforType(element){
  return h[element];
}
function removeChildrenFromVisualTree(elementForDelete) {
    let delet = document.getElementsByClassName(elementForDelete);
    while (delet.length > 0) delet[0].remove();
}
function Event(obj){

  let tipeOfButton = obj.type;
  let elementForDelete =getContenerforType(obj.type);
 
  if(elementForDelete==null){
        console.log(obj.type);
    return 0;
  }
  removeChildrenFromVisualTree(elementForDelete);
  
  
  if(tipeOfButton=='Category'){
    activeItem[tipeOfButton] = obj;
    lastEctiveItem = obj;
    createSubcategory();

  }
  else if(tipeOfButton=='Subcategory'){

    activeItem[tipeOfButton] = obj;
    lastEctiveItem = obj;
    createTeam();

  }
  else
  {
    console.log('None');
  }
}
function fatherfor(type){
    if (type == "Category"){
        return null;
    }
    else if(type == "Subcategory"){
        return activeItem["Category"];
    }
    else if(type == "Team"){
        return activeItem["Subcategory"];
    }
}

function fatherIdfor(type){
    if (type == "Category"){
        return null;
    }
    else if(type == "Subcategory"){
        console.log(activeItem["Category"].id);
        return activeItem["Category"].id;
    }
    else if(type == "Team"){
        console.log(activeItem["Subcategory"].id);
        return activeItem["Subcategory"].id;
    }
} 

function openForm(type) {
    let label = document.getElementById('labe-with-description');
    label.innerHTML= 'Add new '+type.toLowerCase();
    document.getElementById('formid').style = "display: fixed";

    let button = document.getElementById('add-button-id');

    button.onclick= function(e){
        createItem(type);
        e.stopPropagation();
    }
}
    
function createItem(type){

    let name = document.getElementById("item-name-input").value;
        alert(name); 
    let fatherItemId = fatherIdfor(type);
    $.ajax({
      url: '@Url.Page("Index","AddItem")',
      data: { 
          'Type': type,
          'Name':name,
          'FatherItemId': fatherItemId
      }
    }).done(function(date) {

          Event(fatherfor(type));
       });
   document.getElementById('formid').style.display='none';


}

    function createSideLine(type, SizeOfList){
      let Line = document.createElement('div');
      Line.setAttribute('class','SideLine');
      Line.setAttribute('id','SideLine-'+type);
      console.log(SizeOfList);
      Line.style.height = 60*(SizeOfList-1)+'px'
      return Line;
    }

    function createButton(type){
      let Button = document.createElement('div');
      Button.setAttribute('class','button-open-form');
      Button.setAttribute('id','button-open-form-'+type);
      Button.innerHTML='+Add '+type;
      Button.onclick = function(){openForm(type)};
      return Button;
    }

    function сreateList(Date, ItamClass, ItemId, ListElement){
    console.log(Date); 
    let SizeOfList = 0;
      for (let e in Date) {
        let element = Date[e];
        let name = element.name;
        let CategoryListElement = document.createElement('li');
        CategoryListElement.setAttribute('class',ItamClass);
        CategoryListElement.setAttribute('id',ItemId+'-'+name);
        ListElement.appendChild(CategoryListElement);
        
        CategoryListElement.onclick= function(e) {
        Event(element);
        e.stopPropagation();
        }
        
        CategoryListElement.innerHTML=name;
        SizeOfList += 1;
      }
      return SizeOfList;
    }
    
    
    
    function createCategory(){
      let type = 'Category';
      let CategoryDate =  getCategory();
          console.log(CategoryDate); 
      let NameofCategory = 'Category';
      let CategoryContener = document.getElementById('Category-Contener');
      let CategoryList = document.getElementById('Category');
      

      
      CategoryContener.prepend(createButton(type));
      
      сreateList(CategoryDate, "item",NameofCategory,CategoryList);
      
    }
    
    
    function createSubcategory(){
      let type = 'Subcategory';
      let CategoryName = activeItem['Category'].name
      let NameofSubcategory =CategoryName+'-Subcategory';
      
      console.log( activeItem['Category']);
      let SubcategoryDate = getSubcategoryofCategory();
      
      let Subcategory = document.getElementById('Category-'+CategoryName);
     

      
      let SubcategoryContener = document.createElement('div');
      SubcategoryContener.setAttribute('class','Subcategory-Contener');
      SubcategoryContener.setAttribute('id','div-'+NameofSubcategory);
      Subcategory.appendChild(SubcategoryContener);
      
      SubcategoryContener.prepend(createButton(type));
      

      
      
      let SubcategoryList = document.createElement('ul');
      SubcategoryList.setAttribute('id',NameofSubcategory);
      SubcategoryList.setAttribute('class',type);
      SubcategoryContener.appendChild(SubcategoryList);
      
      
      let SizeOfList = сreateList(SubcategoryDate, "item other",type,SubcategoryList);
      
      SubcategoryContener.append(createSideLine(type,SizeOfList));
  }
  
  function createTeam(){
      let type = 'Team';
      let SubcategoryName = activeItem['Subcategory'].name;
      let TeamDate = getTeamofSubcategory();
      let NamaofTeaminSubcategory = SubcategoryName + '-Team';
      let FullName = 'Subcategory'+'-'+SubcategoryName;

      
      let Team = document.getElementById(FullName);
      
      let TeamContener = document.createElement('div');
      TeamContener.setAttribute('class','Team-Contener');
      TeamContener.setAttribute('id','div-'+NamaofTeaminSubcategory);
      Team.appendChild(TeamContener);


      TeamContener.prepend(createButton(type));
      
      let TeamList = document.createElement('ul');
      TeamList.setAttribute('id',NamaofTeaminSubcategory);
      TeamList.setAttribute('class','Team');
      TeamContener.appendChild(TeamList);
      
      let SizeOfList = сreateList(TeamDate, "item other",NamaofTeaminSubcategory,TeamList);
      
      TeamContener.append(createSideLine(type,SizeOfList));
  }


    createCategory();
    var modal = document.getElementById('formid');

    window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}