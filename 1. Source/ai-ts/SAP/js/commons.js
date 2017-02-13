/*
  @author: $$Le Thanh Hung$$
  Start Date: December 05 2006
*/

var GmessageControlId = "message";
var GmessageContainerControlId = "messageContainer";
var GdateFormatString = "mm/dd/yyyy";

function showMessage(msg) {
  if ( msg == null || msg == "" )
    return;

  var ctrlMsgContainer = document.getElementById(GmessageContainerControlId);
  if ( ctrlMsgContainer == null ) {
    alert("There no no control " + GmessageContainerControlId);
    return;
  }
  
  var ctrlMsg = document.getElementById(GmessageControlId);
  if ( ctrlMsg == null ) {
    alert("There no no control " + GmessageControlId);
    return;
  }
  
  ctrlMsg.innerHTML = msg;
  ctrlMsgContainer.style.display = "inline";
}
function hideMessage() {
  var ctrlMsgContainer = document.getElementById(GmessageContainerControlId);
  if ( ctrlMsgContainer == null )
    return;
  ctrlMsgContainer.style.visibility = "hidden";
}
/* pop up window */
function popup(link, windowName, width, height) {
  var property = "width=" + width + ",height=" + height + ",status=no";
  window.open(link, windowName, property);
}

/* Center windows */
function centerScreen() {
  if ( navigator.userAgent.indexOf("Gecko") > 0 ) {
    centerScreenGecko();
  } else {
    centerScreenIE();
  }
}
function centerScreenIE() {
  var w = window.document.body.offsetWidth + 6;
  var h = window.document.body.offsetHeight + 25;
  var W = screen.availWidth;
  var H = screen.availHeight - 20;
  window.moveTo((W - w) / 2, (H - h) / 2);
}
function centerScreenGecko() {
  var w = window.outerWidth;
  var h = window.outerHeight;
  var W = screen.availWidth;
  var H = screen.availHeight - 20;
  window.moveTo((W - w) / 2, (H - h) / 2);
}

function isInteger(s){
	var i;
  for (i = 0; i < s.length; i++){   
    var c = s.charAt(i);
    if (((c < "0") || (c > "9"))) return false;
  }
  return true;
}
function trim(str) {
  return str.replace(/^\s*|\s*$/g,"");
}

/* Form list check box and confirm */

function parentCheckBoxOnclick(parentForm, status, subCheckBoxName)
{                  
    for (var i=0; i<parentForm.elements.length; i++) {            
        if(parentForm.elements[i].type == "checkbox" && parentForm.elements[i].name == subCheckBoxName) {
            parentForm.elements[i].checked = status;
        }
    }
}

function parentCheckBoxOnclickNew(parentForm, status, subCheckBoxName)
{                  
    for (var i=0; i<parentForm.elements.length; i++) {            
        if(parentForm.elements[i].type == "checkbox" && parentForm.elements[i].name.indexOf('deleteRec')) {
            parentForm.elements[i].checked = status;
        }
    }
}

function parentCheckBoxOnclickNeww(parentForm, status, subCheckBoxName)
{                  
    for (var i=0; i<parentForm.elements.length; i++) {            
        if(parentForm.elements[i].type == "checkbox" && parentForm.elements[i].name.indexOf('chkDelete')) {
            parentForm.elements[i].checked = status;
        }
    }
}


function subCheckBoxOnClick(status, parentCheckBox)
{
    if (status == false)
        parentCheckBox.checked = false;
}

function subCheckBoxOnclick(parentForm, parentCheckBoxId, subCheckBoxName) {
    var totalSubCheckBox = 0;
    var totalCheckedSubCheckBox = 0;
    
    for (var i=0; i<parentForm.elements.length; i++) {            
        if(parentForm.elements[i].type == "checkbox" && parentForm.elements[i].name == subCheckBoxName) {
            totalSubCheckBox++;
            var subCheckBox = parentForm.elements[i];
            if (subCheckBox.checked) {
                totalCheckedSubCheckBox++;
            }
        }
    }
    
    var parentCheckBox = document.getElementById(parentCheckBoxId);
    parentCheckBox.checked = (totalCheckedSubCheckBox == totalSubCheckBox);
}

function checkSubCheckBoxIsChecked(form, subCheckBoxName, actionName)
{
    
    var isAnyChecked = false;
    for(var i=0; i<form.elements.length; i++)
        if( form.elements[i].type == "checkbox" && 
            form.elements[i].name == subCheckBoxName   &&
            form.elements[i].checked == true)
            {
                isAnyChecked = true;
                // return true;
            }
    if (isAnyChecked == false)
    {
        alert("Please choose item(s).");
        return false;
    }
    else
    {        
        return confirm("Are you sure you want to " + actionName + " selected item(s)?");        
    }                           
}

function isAnyCheckBoxChecked(form, checkboxName, pleaseChooseItemsLocalize)
{    
    var isAnyChecked = false;
    for(var i=0; i<form.elements.length; i++)
        if( form.elements[i].type == "checkbox" && 
            form.elements[i].name == checkboxName   &&
            form.elements[i].checked == true)
            {
                isAnyChecked = true;
                return true;
            }
            
    if (isAnyChecked == false)
    {
        alert(pleaseChooseItemsLocalize);
        return false;
    }
}

function confirmAction(message) {
    return confirm(message);
}


 function SelectAllCheckboxes(spanChk){

   // Added as ASPX uses SPAN for checkbox
   var oItem = spanChk.children;
   var theBox= (spanChk.type=="checkbox") ? 
        spanChk : spanChk.children.item[0];
   xState=theBox.checked;
   elm=theBox.form.elements;

   for(i=0;i<elm.length;i++)
     if(elm[i].type=="checkbox" && 
              elm[i].id!=theBox.id)
     {
       //elm[i].click();
       if(elm[i].checked!=xState)
         elm[i].click();
       //elm[i].checked=xState;
     }
 }
 
 function check_uncheck(Val)
{
  var ValChecked = Val.checked;
  var ValId = Val.id;
  var frm = document.forms[0];
  // Loop through all elements
  for (i = 0; i < frm.length; i++)
  {
    // Look for Header Template's Checkbox
    //As we have not other control other than checkbox we just check following statement
    if (this != null)
    {
      if (ValId.indexOf('checkAll') !=  - 1)
      {
        // Check if main checkbox is checked,
        // then select or deselect datagrid checkboxes
        if (ValChecked)
          frm.elements[i].checked = true;
        else
          frm.elements[i].checked = false;
      }
      else if (ValId.indexOf('deleteRec') !=  - 1)
      {
        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
        if (frm.elements[i].checked == false)
          frm.elements[1].checked = false;
      }
    } // if
  } // for
} // function

/* End of Form list check box and confirm */

/* Show and hide blocks */
function showBlock(blockIdArray, blockId) {
	for (var i = 0; i < blockIdArray.length; i++) {
		showHideBlock(blockIdArray[i], "none");
	}
	showHideBlock(blockId, "block");
}

function showHideBlock(blockId, display) {
	var block = document.getElementById(blockId);	
	block.style.display = display;
}
/* End of Show and hide blocks */

/* Add/Delete row for connection settings */
function onAddRow()
{
    var newRowNumberHidden = document.getElementById("newRowNumberHidden");
    var index = parseInt(newRowNumberHidden.value) + 1;
	var table = document.getElementById("connectionSettingTable");
	var row = table.insertRow(-1);
	var cell, innerHTML;
		
	cell = row.insertCell(-1);
	cell = row.insertCell(-1);
	innerHTML = '<input type="text" name="newNameText' + index + '" class="Name" />';
	cell.innerHTML = innerHTML;
	
	cell = row.insertCell(-1);
	innerHTML = '<input type="text" name="newValueText' + index + '" class="Value" />';
	cell.innerHTML = innerHTML;
	
	cell = row.insertCell(-1);
	innerHTML = '<input type="text" name="newDescriptionText' + index + '" class="Description" />';
	cell.innerHTML = innerHTML;
	
	newRowNumberHidden.value = index;
}

function onDeleteRow()
{
    var table = document.getElementById("connectionSettingTable");
    var newRowNumberHidden = document.getElementById("newRowNumberHidden");
    var count = parseInt(newRowNumberHidden.value) 
    
	if (count > 0) {
		table.deleteRow(-1);
		newRowNumberHidden.value = count - 1;
	}
}

function deleteSelectedRows() {
    if (checkSubCheckBoxIsChecked(document.forms[0], "objID", "delete")) {
        var objCheckBoxs = document.getElementsByName("objID");
        for (var i = objCheckBoxs.length - 1; i >= 0; i--) {
            var objCheckBox = objCheckBoxs[i];
            if (objCheckBox.checked) {
                var row = objCheckBox.parentNode.parentNode;                
                var table = row.parentNode;                
                table.deleteRow(row.rowIndex - 1);
                var checkAll = document.getElementById("parentCheckBox");                
            }
        }
        checkAll.checked = false;
    }    
}

function initHiddenField() {
    var newRowNumberHidden = document.getElementById("newRowNumberHidden");
    newRowNumberHidden.value = 0;
}


/* End of Add/Delete row for connection settings */

/* Insert text at cursor */

function insertAtCursor(myField, myValue) {
    //IE support
    if (document.selection) {
        myField.focus();        
        var sel = document.selection.createRange();        
        sel.text = myValue;
    }
    //MOZILLA/NETSCAPE support
    else if (myField.selectionStart || myField.selectionStart == '0') {
        var startPos = myField.selectionStart;
        var endPos = myField.selectionEnd;
        myField.value = myField.value.substring(0, startPos) + myValue + myField.value.substring(endPos, myField.value.length);
    } 
    else {
        myField.value += myValue;
    }
}

function insertAtCursorBySelect(selectID, fieldID, prefix, suffix) {
    var select = document.getElementById(selectID);
    var field = document.getElementById(fieldID);
    var value = select.value;
    if (prefix != undefined) { value = prefix + value; }
    if (suffix != undefined) { value = value + suffix; }
    insertAtCursor(field, value);
    field.focus();
}

/* End of Insert text at cursor */

/*
    Check In Thread in EditConnectionSession Page
*/
function doCheckThread(inThreadCheckBox, autoThreadCheckBox, thresholdTextBox) {
    if (inThreadCheckBox.checked)
    {
        alert('It will run in separate thread!');
        
        autoThreadCheckBox.disabled = true;            
        thresholdTextBox.disabled = true;
    }
    else
    {
        alert('It will run in common thread!');
        autoThreadCheckBox.disabled = false;
        thresholdTextBox.disabled = false;            
    }
}

function CallEvent(evt, target, type){
        $(target).bind(evt, function() {   
                var text = $(this).attr("value");
                var keyCode;
                if(text !=null && text!='undefined'){
                    if(type == 'int')
                    {
                        if(!isInteger(text))
                        {
                            text = text.substr(0, text.length - 1);
                            $(this).attr("value",text);
                        }
                     }
                    if(type == 'float')
                    {
                        if(!isValidInputFloat($.trim(text)))
                        {
                            text = text.substr(0, text.length - 1);
                            $(this).attr("value",text);
                        }
                     }
                }
        });
    }

    function LimitInputInteger(target){
        CallEvent('keyup', target, 'int');
        CallEvent('click', target, 'int');
        CallEvent('mouseup', target, 'int');
        CallEvent('blur', target, 'int');
        CallEvent('input', target, 'int');
    }

