
/* code from qodo.co.uk */
// create as many regular expressions here as you need:
var digitsOnly = /[0-9]/g;
var integerOnly = /[0-9\.\,]/g;
var alphaOnly = /[A-Za-zñÑáéíóúÁÉÍÓÚüÜ\s]/g;
var alphaNumericOnly = /[A-Za-z0-9ñÑáéíóúÁÉÍÓÚüÜ]/g;
var forbidden = /[\!\@\#\$\%\^\&\*\(\)]/g;
var timeFormat = /[0-9:]/g; //HH:mm

function restrictCharacters(myfield, e, restrictionType, maxLength) {
    if (!e) var e = window.event
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);

    // if they pressed esc... remove focus from field...
    if (code == 27) { this.blur(); return false; }

    // ignore if they are press other keys
    // strange because code: 39 is the down key AND ' key...
    // and DEL also equals .

    if (character.match(forbidden)) {
        return false;
    }

    if (!e.ctrlKey && code != 9 && code != 8 && code != 46 && code != 36 && code != 37 && code != 38 && (code != 39 || (code == 39 && character == "'")) && code != 40) {
        if (character.match(restrictionType)) {
            if (myfield.value.length > maxLength - 1)
                return false;
            else
                return true;
        } else {
            return false;
        }
    }
}