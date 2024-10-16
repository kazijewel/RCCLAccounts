function isEmpty(obj) {
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            return false;
        }
    }
    return true;
}
// Amount/Number Field Start
function number(e) {
    e = e || window.event;
    var charCode = e.which ? e.which : e.keyCode;
    return /\d/.test(String.fromCharCode(charCode));
}
function amount(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
function amountWithComma(debitAmount) {
    var val = $(debitAmount).val();
    val = val.replace(/[^0-9\.]/g, '');

    if (val != "") {
        valArr = val.split('.');
        valArr[0] = (parseInt(valArr[0], 10)).toLocaleString();
        val = valArr.join('.');
    }
    $(debitAmount).val(val);
}
function amountShowWithComma(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
// Amount/Number Field End
//Date Format Change Start
function getBdToDbFormat(date) {
   
    //var x = date.split("-");
    //var day = x[0];
    //var month = x[1];
    //var year = x[2];
    //return year + '-' + month + '-' + day;
    //let [day, month, year] = date.split("-");
    //return `${year}-${month}-${day}`;

    let year = date.getFullYear();
    let month = ('0' + (date.getMonth() + 1)).slice(-2); // Add leading zero for single digit months
    let day = ('0' + date.getDate()).slice(-2); // Add leading zero for single digit days

    return `${year}-${month}-${day}`;
   
   
}
function getDbToBdFormat(date) {
    var x = date.split("-");
    var day = x[2];
    var month = x[1];
    var year = x[0];
    return day + '-' + month + '-' + year;
}
//Date Format Change End
//Get Full Age Start
function getAgeFull(birthDate, toDate) {
    if (!isNaN(birthDate) || !isNaN(toDate)) {
        return "";
    }
    var birthdate = birthDate;
    var senddate = toDate;
    var x = birthdate.split("-");
    var y = senddate.split("-");
    var bdays = x[0];
    var bmonths = x[1];
    var byear = x[2];
    var sdays = y[0];
    var smonths = y[1];
    var syear = y[2];
    if (sdays < bdays) {
        sdays = parseInt(sdays) + 30;
        smonths = parseInt(smonths) - 1;
        var fdays = sdays - bdays;
    }
    else {
        var fdays = sdays - bdays;
    }
    if (smonths < bmonths) {
        smonths = parseInt(smonths) + 12;
        syear = syear - 1;
        var fmonths = smonths - bmonths;
    }
    else {
        var fmonths = smonths - bmonths;
    }

    var fyear = syear - byear;
    console.log(fyear + '>' + fmonths + '>' + fdays);
    return fyear + 'Y. ' + fmonths + 'M. ' + fdays + 'D';
}
//Get Full Age End
//For New Date Start
function getCDay() {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    return dd;
}
function getCMonth() {
    var today = new Date();
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    return mm;
}
function getCYear() {
    var dateobj = new Date();
    var year = dateobj.getFullYear();
    return year;
}

function addDay(date, add) {
    return new Date(date.setDate(date.getDate() + add));
}
function subDay(date, sub) {
    return new Date(date.setDate(date.getDate() - sub));
}
function addMonth(date, add) {
    return new Date(date.setMonth(date.getMonth() + add));
}
function subMonth(date, sub) {
    return new Date(date.setMonth(date.getMonth() - sub));
}
function addYear(date, add) {
    return new Date(date.setFullYear(date.getFullYear() + add));
}
function subYear(date, sub) {
    return new Date(date.setFullYear(date.getFullYear() - sub));
}
//For New Date End
document.onkeydown = function (e) {
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'S'.charCodeAt(0)) {
        $('#btnSave').click();
    }
}
document.onkeydown = function (e) {
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'E'.charCodeAt(0)) {
        $('#btnEdit').click();
    }
}
// Focus enter Start
/*$(document).on('keypress', 'form input,select', function (event) {
    event.stopImmediatePropagation();
    if (event.which == 13) {
        event.preventDefault();
        var $input = $('form input,select');
        if ($(this).is($input.last())) {
            $('#btnSave').focus();
        }
        else {
            $input.eq($input.index(this) + 1).focus();
        }
    }
})*/

function focusScript(event, ele) {
    //See notes about 'which' and 'key'
    if (event.which == 13 || event.keyCode == 13) {
       // console.log(ele)
        $(ele).focus();
        $(ele).select();
        return false;
    }
    return true;
}
// Focus Enter End
$(document).ready(function () {
    $('.datepick').datepicker({
        format: "dd-mm-yyyy",
        autoclose: true,
        disableTouchKeyboard: true,
        todayHighlight: true
    });
    $('.datetimepick').date

});
$.fn.dataTable.render.moment = function (from, to, locale) {
    // Argument shifting
    if (arguments.length === 1) {
        locale = 'en';
        to = from;
        from = 'YYYY-MM-DD';
    }
    else if (arguments.length === 2) {
        locale = 'en';
    }

    return function (d, type, row) {
        if (!d) {
            return type === 'sort' || type === 'type' ? 0 : d;
        }

        var m = window.moment(d, from, locale, true);

        // Order and type get a number value from Moment, everything else
        // sees the rendered value
        return m.format(type === 'sort' || type === 'type' ? 'x' : to);
    };
};


///** -------------------------------------- New Auto complete --------------------------------------**////

    function autocomplete(inp, arr) {


        /*the autocomplete function takes two arguments,
        the text field element and an array of possible autocompleted values:*/
        var currentFocus;
        /*execute a function when someone writes in the text field:*/

        function visibleAllElement(a) {
            var x = a;
            if (x) x = x.getElementsByTagName("div");
            if (inp.value != "") {
                for (i = 0; i < arr.length; i++) {
                    x[i].style.cssText = 'visibility: visible;';
                }
            }
        }

        function bElement(a, val) {
            if (val != "") {
                for (i = 0; i < arr.length; i++) {
                    b = document.createElement("DIV");

                    if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                        b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                        b.innerHTML += arr[i].substr(val.length);
                        b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";

                        b.addEventListener("click", function (e) {
                            inp.value = this.getElementsByTagName("input")[0].value;
                            closeAllLists();
                        });
                        a.appendChild(b);

                        a.addEventListener("mousewheel", function (enrt) {
                            visibleAllElement(a);
                        });
                        a.addEventListener("mouseenter", function (enrt) {
                            visibleAllElement(a);
                        });
                    }
                }
            }
            else {
                for (i = 0; i < arr.length; i++) {
                    for (i = 0; i < arr.length; i++) {
                        b = document.createElement("DIV");

                        b.innerHTML += "<input type='hidden' value='" + arr[i] + "'><lable>" + arr[i] + "</lable>";

                        b.addEventListener("click", function (e) {
                            inp.value = this.getElementsByTagName("input")[0].value;
                            closeAllLists();
                        });
                        a.appendChild(b);

                        a.addEventListener("mousewheel", function (enrt) {
                            visibleAllElement(a);
                        });
                        a.addEventListener("mouseenter", function (enrt) {
                            visibleAllElement(a);
                        });
                    }
                }
            }
        }


        inp.addEventListener("input", function abc(e) {
            var a, val = this.value;
            closeAllLists();
            currentFocus = -1;

            a = document.createElement("DIV");
            a.setAttribute("id", this.id + "autocomplete-list");
            a.setAttribute("class", "autocomplete-items");
            this.parentNode.appendChild(a);

            bElement(a, val);
        });



        inp.addEventListener("keydown", function def(e) {
            var InpValue = inp.value;
            var code = e.keyCode;
            if (InpValue != "") { }
            else if (code == 40 || code == 38) {
                closeAllLists();
                currentFocus = -1;
                var a;
                a = document.createElement("DIV");
                a.setAttribute("id", this.id + "autocomplete-list");
                a.setAttribute("class", "autocomplete-items");

                this.parentNode.appendChild(a);
                bElement(a, this.value);
            }





            var x = document.getElementById(this.id + "autocomplete-list");
            if (x) x = x.getElementsByTagName("div");
            if (code == 40) { // Down

                currentFocus++;

                addActive(x);
                if (typeof arr[currentFocus] === "undefined") {
                    inp.value = "";
                }
                else {
                    inp.value = arr[currentFocus];
                }

                if (!isNaN(currentFocus)) {
                    if (currentFocus > 5) {
                        keyFocus = currentFocus - 5;
                        for (j = 0; j < keyFocus; j++) {
                            x[j].style.cssText = 'visibility: hidden; height: 0px; padding: 0px; border: 0px;';
                        }
                    }
                    else if (currentFocus == 0) {
                        for (j = 0; j < arr.length; j++) {
                            x[j].style.cssText = 'visibility: visible;';
                        }
                    }
                    else {
                        x[currentFocus].style.cssText = 'visibility: visible;';
                    }
                }
            }
            else if (code == 38) { //up

                currentFocus--;
                addActive(x);

                if (typeof arr[currentFocus] === "undefined") {
                    inp.value = "";
                }
                else {
                    inp.value = arr[currentFocus];
                }

                if (currentFocus > 5) {
                    keyFocus = currentFocus - 5;
                    for (j = 0; j < keyFocus; j++) {
                        x[j].style.cssText = 'visibility: hidden; height: 0px; padding: 0px; border: 0px;';
                    }
                }
                else {
                    x[currentFocus].style.cssText = 'visibility: visible;';
                }


            }
            else if (code == 13) { //enter
                e.preventDefault();
                if (currentFocus > -1) {
                    if (x) x[currentFocus].click();
                }
            }
        });

        function addActive(x) {

            /*a function to classify an item as "active":*/
            if (!x) return false;
            /*start by removing the "active" class on all items:*/
            removeActive(x);
            if (currentFocus >= x.length) currentFocus = 0;
            if (currentFocus < 0) currentFocus = (x.length - 1);

            console.log("Current Focus Work: " + currentFocus);
            if (isNaN(currentFocus)) {
                x[0].classList.add("autocomplete-active");
                currentFocus = 0;
            }
            else {
                x[currentFocus].classList.add("autocomplete-active");
            }
        }
        function removeActive(x) {
            /*a function to remove the "active" class from all autocomplete items:*/
            for (var i = 0; i < x.length; i++) {
                x[i].classList.remove("autocomplete-active");
            }
        }
        function closeAllLists(elmnt) {
            /*close all autocomplete lists in the document,
            except the one passed as an argument:*/
            var x = document.getElementsByClassName("autocomplete-items");
            for (var i = 0; i < x.length; i++) {
                if (elmnt != x[i] && elmnt != inp) {
                    console.log('aa');
                    x[i].parentNode.removeChild(x[i]);
                }
            }
        }

        /*execute a function when someone clicks in the document:*/
        document.addEventListener("click", function (e) {
            closeAllLists(e.target);
        });


    }


    function clearAutoSuggestData() {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        //inp.value = "";
        var x = document.getElementsByClassName("autocomplete-items");
        console.log(x.parentNode)
    }




///** -------------------------------------- Old Auto complete --------------------------------------**////

/*function autocomplete(inp, arr) {
    *//*the autocomplete function takes two arguments,
the text field element and an array of possible autocompleted values:*//*
var currentFocus;
*//*execute a function when someone writes in the text field:*//*
inp.addEventListener("input", function (e) {
    var a, b, i, val = this.value;
    *//*close any already open lists of autocompleted values*//*
closeAllLists();
if (!val) { return false; }
currentFocus = -1;
*//*create a DIV element that will contain the items (values):*//*
a = document.createElement("DIV");
a.setAttribute("id", this.id + "autocomplete-list");
a.setAttribute("class", "autocomplete-items");
*//*append the DIV element as a child of the autocomplete container:*//*
this.parentNode.appendChild(a);
*//*for each item in the array...*//*
for (i = 0; i < arr.length; i++) {
    *//*check if the item starts with the same letters as the text field value:*//*
if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
    *//*create a DIV element for each matching element:*//*
b = document.createElement("DIV");
*//*make the matching letters bold:*//*
b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
b.innerHTML += arr[i].substr(val.length);
*//*insert a input field that will hold the current array item's value:*//*
b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
*//*execute a function when someone clicks on the item value (DIV element):*//*
b.addEventListener("click", function (e) {
    *//*insert the value for the autocomplete text field:*//*
inp.value = this.getElementsByTagName("input")[0].value;
*//*close the list of autocompleted values,
(or any other open lists of autocompleted values:*//*
closeAllLists();
});
a.appendChild(b);
}
}
});
*//*execute a function presses a key on the keyboard:*//*
inp.addEventListener("keydown", function (e) {
    var x = document.getElementById(this.id + "autocomplete-list");
    if (x) x = x.getElementsByTagName("div");
    if (e.keyCode == 40) {
        *//*If the arrow DOWN key is pressed,
increase the currentFocus variable:*//*
currentFocus++;
*//*and and make the current item more visible:*//*
addActive(x);
} else if (e.keyCode == 38) { //up
*//*If the arrow UP key is pressed,
decrease the currentFocus variable:*//*
currentFocus--;
*//*and and make the current item more visible:*//*
addActive(x);
} else if (e.keyCode == 13) {
*//*If the ENTER key is pressed, prevent the form from being submitted,*//*
e.preventDefault();
if (currentFocus > -1) {
    *//*and simulate a click on the "active" item:*//*
if (x) x[currentFocus].click();
}
}
});
function addActive(x) {
*//*a function to classify an item as "active":*//*
if (!x) return false;
*//*start by removing the "active" class on all items:*//*
removeActive(x);
if (currentFocus >= x.length) currentFocus = 0;
if (currentFocus < 0) currentFocus = (x.length - 1);
*//*add class "autocomplete-active":*//*
x[currentFocus].classList.add("autocomplete-active");
}
function removeActive(x) {
*//*a function to remove the "active" class from all autocomplete items:*//*
for (var i = 0; i < x.length; i++) {
    x[i].classList.remove("autocomplete-active");
}
}
function closeAllLists(elmnt) {
*//*close all autocomplete lists in the document,
except the one passed as an argument:*//*
var x = document.getElementsByClassName("autocomplete-items");
for (var i = 0; i < x.length; i++) {
    if (elmnt != x[i] && elmnt != inp) {
        x[i].parentNode.removeChild(x[i]);
    }
}
}
*//*execute a function when someone clicks in the document:*//*
document.addEventListener("click", function (e) {
    closeAllLists(e.target);
});*/



