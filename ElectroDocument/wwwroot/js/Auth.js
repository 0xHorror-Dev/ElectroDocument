var tokenKey = "accessToken";

document.getElementById("loginSubmit").addEventListener("click", async e => {
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

        //sessionStorage.setItem(tokenKey, data.accessToken);

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
