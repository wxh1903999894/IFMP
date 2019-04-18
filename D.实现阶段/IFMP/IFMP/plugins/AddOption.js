
function Fun_AddOption()
{
 var table=document.getElementById("addoption");
   var newTr=table.insertRow(); 
   var newTd1=newTr.insertCell();
   var newTd2=newTr.insertCell();
  newTd1.innerHTML +="<input type='text' name='option' size='100'/>";
  newTd2.innerHTML += "<img src='../images/allpic.png' onclick='Fun_AddOption()' />&nbsp;&nbsp;";
  newTd2.innerHTML += "<img src='../images/allinpic.png' onclick='Fun_DelOption(this.value)'/>";
}
function Fun_DelOption(a)
{
  var Row=window.event.srcElement.parentElement.parentElement.rowIndex;  
  var table=document.getElementById("addoption");  
  table.deleteRow(Row); 
}

function fun_Show(obj)
{ 
  var div = document.getElementById("t_view")
  var value=obj.options[obj.selectedIndex].text;
  if(value == "问答题")
  {
   div.style.display="none";
  }
 else div.style.display="block";
}