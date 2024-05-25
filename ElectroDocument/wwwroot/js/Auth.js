var tokenKey = "accessToken";
function isEmpty(input) {
    return input.trim().length === 0;
}

document.getElementById("loginSubmit").addEventListener("click", async e => {
    if (isEmpty(document.getElementById("username").value)) {
        if (isEmpty(document.getElementById("password").value)) {
            document.getElementById("ErrorNotification").innerHTML = "Введите логин и пароль!";
        }
        else {
            document.getElementById("ErrorNotification").innerHTML = "Введите логин!";

        }

        
        return;
    }
    else if (isEmpty(document.getElementById("password").value))
    {
        document.getElementById("ErrorNotification").innerHTML = "Введите пароль!";
        return;
    }

    e.preventDefault();
    let response = await fetch("/Auth/Login", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            Username: document.getElementById("username").value,
            Password: document.getElementById("password").value
        })
    });

    var responseCopy = response.clone();


    if (response.ok === true) {

        const data = await responseCopy.json();

        console.log(data.accessToken)

        const response = await fetch("/Profile", {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + data.accessToken
            }
        });

        console.log(response)

        document.location.href = '/';

    }
    else {
        document.getElementById("password").innerHTML = "";
        document.getElementById("ErrorNotification").innerHTML = "Неверный логин или пароль";
    }
});
