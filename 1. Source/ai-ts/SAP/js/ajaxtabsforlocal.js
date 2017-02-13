var bustcachevar=1 //bust potential caching of external pages after initial request? (1=yes, 0=no)
var loadstatustext="<img src='../images/loading.gif' /> Requesting content..."

////NO NEED TO EDIT BELOW////////////////////////
var loadedobjects=""
var defaultcontentarray=new Object()
var bustcacheparameter=""

function ajaxpage(url, containerid, targetobj) {
        
    var ullist = targetobj.parentNode.parentNode.getElementsByTagName("li")
    for (var i=0; i<ullist.length; i++)
        ullist[i].className=""  //deselect all tabs
        
    targetobj.parentNode.className="selected"  //highlight currently clicked on tab
}

function loadpage(page_request, containerid){
  if (page_request.readyState == 4 && (page_request.status==200 || window.location.href.indexOf("http")==-1))
  {
    alert(page_request.responseText);
    document.getElementById(containerid).innerHTML=page_request.responseText;
  }
}

function loadobjs(revattribute) {
    if (revattribute!=null && revattribute!="") { //if "rev" attribute is defined (load external .js or .css files)
        var objectlist=revattribute.split(/\s*,\s*/) //split the files and store as array
        for (var i=0; i<objectlist.length; i++) {
            var file=objectlist[i]
            var fileref=""
            if (loadedobjects.indexOf(file)==-1) { //Check to see if this object has not already been added to page before proceeding
                if (file.indexOf(".js")!=-1) { //If object is a js file
                    fileref=document.createElement('script')
                    fileref.setAttribute("type","text/javascript");
                    fileref.setAttribute("src", file);
                }
                else if (file.indexOf(".css")!=-1) { //If object is a css file
                    fileref=document.createElement("link")
                    fileref.setAttribute("rel", "stylesheet");
                    fileref.setAttribute("type", "text/css");
                    fileref.setAttribute("href", file);
                }
            }
            if (fileref!="") {
                document.getElementsByTagName("head").item(0).appendChild(fileref)
                loadedobjects+=file+" " //Remember this object as being already added to page
            }
        }
    }
}

function expandtab(tabcontentid, tabnumber){ //interface for selecting a tab (plus expand corresponding content)
  var thetab=document.getElementById(tabcontentid).getElementsByTagName("a")[tabnumber];
  if (thetab.getAttribute("rel")) {
    ajaxpage(thetab.getAttribute("href"), thetab.getAttribute("rel"), thetab);
    loadobjs(thetab.getAttribute("rev"));
  }
}

function savedefaultcontent(contentid) {// save default ajax tab content
  if (typeof defaultcontentarray[contentid]=="undefined") //if default content hasn't already been saved
  defaultcontentarray[contentid]=document.getElementById(contentid).innerHTML
}

function startajaxtabs(){
  for (var i=0; i<arguments.length; i++) { //loop through passed UL ids
    var ulobj=document.getElementById(arguments[i])
    var ulist=ulobj.getElementsByTagName("li") //array containing the LI elements within UL
    for (var x=0; x<ulist.length; x++) { //loop through each LI element
      var ulistlink=ulist[x].getElementsByTagName("a")[0];
      if (ulistlink.getAttribute("rel")) {
        var modifiedurl = ulistlink.getAttribute("href").replace(/^http:\/\/[^\/]+\//i, "http://"+window.location.hostname+"/")
        ulistlink.setAttribute("href", modifiedurl) //replace URL's root domain with dynamic root domain, for ajax security sake
        //savedefaultcontent(ulistlink.getAttribute("rel")); //save default ajax tab content
        ulistlink.onclick = function(x) {  
            //alert(x);               
          var href = this.getAttribute("href");
          ajaxpage(this.getAttribute("href"), this.getAttribute("rel"), this);
          var form = document.forms[0];
          var field = document.getElementById('reportTypeHidden');
          var tabSelected = document.getElementById('tabSelectedHidden');          
          if (href.indexOf("#10") >= 0) {
            field.value = 'ByCarrierStatus';
            tabSelected.value = '10';  
            return;
          }
          else if (href.indexOf("#11") >= 0) {
            field.value = 'ByService';
            tabSelected.value = '11';  
            alert('Not implemented yet!');
            return;
          }
          else if (href.indexOf("#12") >= 0) {
            field.value = 'ByService';
            tabSelected.value = '12';  
            alert('Not implemented yet!');
            return;
          }
          else if (href.indexOf("#13") >= 0) {
            field.value = 'ByHour';
            tabSelected.value = '13';
          }
          else if (href.indexOf("#14") >= 0) {
            field.value = 'ByDate';
            tabSelected.value = '14';
          }
          else if (href.indexOf("#15") >= 0) {
            field.value = 'ByCarrier';
            tabSelected.value = '15';
          }
          else if (href.indexOf("#16") >= 0) {
            field.value = 'ByCarrierStatus';
            tabSelected.value = '16';
          }
          else if (href.indexOf("#1") >= 0)  { 
            field.value = 'ByService';
            tabSelected.value = '1';            
          }          
          else if (href.indexOf("#2") >= 0) {
            field.value = 'ByService';
            tabSelected.value = '2';
          }
          else if (href.indexOf("#3") >= 0) {
            field.value = 'TotalByStatus';
            tabSelected.value = '3';  
          }
          else if (href.indexOf("#4") >= 0) {
            field.value = 'TotalByStatusDate';
            tabSelected.value = '4';  
          }
          else if (href.indexOf("#5") >= 0) {
            field.value = 'ByService';
            tabSelected.value = '5';  
          }
          else if (href.indexOf("#6") >= 0) {
            field.value = 'ByService';
            tabSelected.value = '6';  
          }
          else if (href.indexOf("#7") >= 0) {
            field.value = 'ByService';
            tabSelected.value = '7';  
          }
          else if (href.indexOf("#8") >= 0) {
            field.value = 'ByWorkWeekend';
            tabSelected.value = '8';
          }
          else if (href.indexOf("#9") >= 0) {
            field.value = 'ByOrigin';
            tabSelected.value = '9';
          }
          form.submit();
        }
        if (ulist[x].className=="selected") {
          ajaxpage(ulistlink.getAttribute("href"), ulistlink.getAttribute("rel"), ulistlink) //auto load currenly selected tab content
          loadobjs(ulistlink.getAttribute("rev")) //auto load any accompanying .js and .css files
        }
      }
    }
  }
}

function checkReportByDate() {
    var display;
    var tabSelectedHidden = document.getElementById("tabSelectedHidden");
    if (tabSelectedHidden.value == "4" || tabSelectedHidden.value == "12") {
        display = "none";
    } 
    else {
        display = "inline";
    }
    var toDateSpan = document.getElementById("toDateSpan");
    toDateSpan.style.display = display;
}

function checkReportByStatus(mtRadioId) {    
    var incomingDisplay, outgoingDisplay;
    var tabSelectedHidden = document.getElementById("tabSelectedHidden");
    if (tabSelectedHidden.value == "2" || tabSelectedHidden.value == "10" || tabSelectedHidden.value == "16") {
        var mtRadio = document.getElementById(mtRadioId);        
        if (mtRadio.checked) {
            incomingDisplay = "none";
            outgoingDisplay = "inline";
        }
        else {
            incomingDisplay = "inline";
            outgoingDisplay = "none";
        }
    }    
    else {
        incomingDisplay = outgoingDisplay = "none";
    }
    var incomingStatusSpan = document.getElementById("incomingStatusSpan");
    var outgoingStatusSpan = document.getElementById("outgoingStatusSpan");
    incomingStatusSpan.style.display = incomingDisplay;
    outgoingStatusSpan.style.display = outgoingDisplay;
}

function reloadBarChartFrame() {
    var frame = document.getElementById("RSIFrame");
    frame.src = frame.src;
}

function downloadBarChart() {    
    var newWindow = window.open('StatisticsImageGraphForLocal.aspx?mode=download','Chart','height=0,width=0,menubar=no,scrollbars=no,resizable=no,title=no,statusbar=no');    	
}

