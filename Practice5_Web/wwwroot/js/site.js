// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var updateDataAccess = value => {
    fetch('/Home/DataAccess', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(value)
    })
        .then(res => res.json())
        .then(data => console.log(data))
        .catch(error => console.error(error));
}

document.addEventListener('DOMContentLoaded', () => {
    fetch('/Home/DataAccess')
        .then(res => res.json())
        .then(data => {
            if (data.dataAccess) {
                document.getElementById('dataAccessSelector').value = data.dataAccess;
            }
        })
        .catch(error => console.error(error));
})
