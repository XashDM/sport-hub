const  h={
      'Category':'Subcategory-Contener',
      'Subcategory':'Team-Contener',
      'Team': null
    };
    let EctiveItem = {
      'Category': null,
      'Subcategory':null,
      'Team': null
    }
   let CategoryFDateList= null;
    let SubcategoryDateList= null;
   let TeamDateList= null;

let EctiveCategory = '';
let EctiveSubCategory = '';
let LastEctiveItem = null;
function GetTeamofSubcategory(){
    let  Date;
    obj=EctiveItem['Subcategory'];
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
          Date = date;
       });
    return Date;
}
function GetSubcategoryofCategory(){
    let  Date;
    obj=EctiveItem['Category'];
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
          Date = date;
       });

   return Date;
}
function GetCategory(){ 
    let  Date;
    $.ajax({
          url: '@Url.Page("Index","Root")',
          data : $("#tab"),
          async: false
       }).done(function(date) {
          console.log(date);
          Date = date;
       });

   return Date;
}

function pars(id){
  var partsArray = id.split('-');
  return partsArray
}
function translete(element){
  return h[element];
}
function Event(obj){
  console.log(obj);
  let TipeOfButton = obj.type;
  let ElementForDelete =translete(obj.type);
 
  if(ElementForDelete==null){
        console.log(obj.type);
    return 0;
  }
  console.log(obj);
  let delet = document.getElementsByClassName(ElementForDelete);
  while (delet.length > 0) delet[0].remove();
  
  
  if(TipeOfButton=='Category'){
    EctiveItem[TipeOfButton] = obj;
    LastEctiveItem = obj;
    CreateSubcategory();

  }
  else if(TipeOfButton=='Subcategory'){

    EctiveItem[TipeOfButton] = obj;
    LastEctiveItem = obj;
    CreateTeam();

  }
  else
  {
          console.log('None');
  }
}
    function Fatherfor(type){
        if (type == "Category")
        {
            return null;
        }
        else if(type == "Subcategory")
        {
            return EctiveItem["Category"];
        }
        else if(type == "Team")
        {
            return EctiveItem["Subcategory"];
        }
    }
    function FatherIdfor(type){
        alert(type); 
        if (type == "Category")
        {
            return null;
        }
        else if(type == "Subcategory")
        {
            console.log(EctiveItem["Category"].id);
             return EctiveItem["Category"].id;

        }
        else if(type == "Team")
        {
            console.log(EctiveItem["Subcategory"].id);
            return EctiveItem["Subcategory"].id;
        }
    } 

    function OpenForm(type){
    let label = document.getElementById('labe-with-description');
    label.innerHTML= 'Add new '+type.toLowerCase();
    document.getElementById('formid').style = "display: fixed";
    let button = document.getElementById('add-button-id');
    button.onclick= function(e){
        CreateItem(type);
        e.stopPropagation();
        }
    }
    
    function CreateItem(type){

    let name = document.getElementById("item-name-input").value;
        alert(name); 
    let FatherItemId = FatherIdfor(type);
    $.ajax({
      url: '@Url.Page("Index","AddItem")',
      data: { 
          'Type': type,
          'Name':name,
          'FatherItemId': FatherItemId
           }
   }).done(function(date) {
      alert(date); 

      Event(Fatherfor(type));
   });
   document.getElementById('formid').style.display='none';


   }

    function CreateSideLine(type, SizeOfList){
      let Line = document.createElement('div');
      Line.setAttribute('class','SideLine');
      Line.setAttribute('id','SideLine-'+type);
      console.log(SizeOfList);
      Line.style.height = 60*(SizeOfList-1)+'px'
      return Line;
    }

    function CreateButton(type){
      let Button = document.createElement('div');
      Button.setAttribute('class','button-open-form');
      Button.setAttribute('id','button-open-form-'+type);
      Button.innerHTML='+Add '+type;
      Button.onclick = function(){OpenForm(type)};
      return Button;
    }

    function CreateList(Date, ItamClass, ItemId, ListElement){
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
    
    
    
    function CreateCategory(){
      let type = 'Category';
      let CategoryDate =  GetCategory();
          console.log(CategoryDate); 
      let NameofCategory = 'Category';
      let CategoryContener = document.getElementById('Category-Contener');
      let CategoryList = document.getElementById('Category');
      

      
      CategoryContener.prepend(CreateButton(type));
      
      CreateList(CategoryDate, "item",NameofCategory,CategoryList);
      
    }
    
    
    function CreateSubcategory(){
      let type = 'Subcategory';
      let CategoryName = EctiveItem['Category'].name
      let NameofSubcategory =CategoryName+'-Subcategory';
      
      console.log( EctiveItem['Category']);
      let SubcategoryDate = GetSubcategoryofCategory();
      
      let Subcategory = document.getElementById('Category-'+CategoryName);
     

      
      let SubcategoryContener = document.createElement('div');
      SubcategoryContener.setAttribute('class','Subcategory-Contener');
      SubcategoryContener.setAttribute('id','div-'+NameofSubcategory);
      Subcategory.appendChild(SubcategoryContener);
      
      SubcategoryContener.prepend(CreateButton(type));
      

      
      
      let SubcategoryList = document.createElement('ul');
      SubcategoryList.setAttribute('id',NameofSubcategory);
      SubcategoryList.setAttribute('class',type);
      SubcategoryContener.appendChild(SubcategoryList);
      
      
      let SizeOfList = CreateList(SubcategoryDate, "item other",type,SubcategoryList);
      
      SubcategoryContener.append(CreateSideLine(type,SizeOfList));
  }
  
  function CreateTeam(){
      let type = 'Team';
      let SubcategoryName = EctiveItem['Subcategory'].name;
      let TeamDate = GetTeamofSubcategory();
      let NamaofTeaminSubcategory = SubcategoryName + '-Team';
      let FullName = 'Subcategory'+'-'+SubcategoryName;

      
      let Team = document.getElementById(FullName);
      
      let TeamContener = document.createElement('div');
      TeamContener.setAttribute('class','Team-Contener');
      TeamContener.setAttribute('id','div-'+NamaofTeaminSubcategory);
      Team.appendChild(TeamContener);


      TeamContener.prepend(CreateButton(type));
      
      let TeamList = document.createElement('ul');
      TeamList.setAttribute('id',NamaofTeaminSubcategory);
      TeamList.setAttribute('class','Team');
      TeamContener.appendChild(TeamList);
      
      let SizeOfList = CreateList(TeamDate, "item other",NamaofTeaminSubcategory,TeamList);
      
      TeamContener.append(CreateSideLine(type,SizeOfList));
  }


    CreateCategory();
    var modal = document.getElementById('formid');

    window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}