import 'jquery';
import 'bootstrap';
import '../node_modules/bootstrap/dist/css/bootstrap.css'
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