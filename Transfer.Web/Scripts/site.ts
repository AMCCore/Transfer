import 'jquery';
import 'bootstrap';
import 'jquery-ajax-unobtrusive';

import '../node_modules/bootstrap/dist/css/bootstrap.css';
import '../Styles/site.css';

import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';

let w;
w = window;

w.jQuery = $;
w.$ = $;

function TSButton() {
    let name = "Fred";
    $('.ts-example').html(greeter(user));
}

class Student {
    fullName;
    constructor(firstName, middleInitial, lastName) {
        this.fullName = firstName + " z " + middleInitial + " " + lastName;
    }
}

function greeter(person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}

let user = new Student("Fred", "M.", "Smith");

function SomeButtonZero() {
    let name = "Zusul";
    
    alert(name);
}

function LoginRefirect(r) {
    var e = r.responseJSON;
    if (e.redirect) {
        //window.location.href = e.redirect;
        alert(e.redirect);
    }
}