document.getElementById("addCart").addEventListener("click", function () {
    var data = { productID: document.getElementById("addCart").getAttribute("data-productID") }
    url = "../Shopping/AddCart"
    fetch(url,
        {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        }
    )
        .then(Response => {
            return Response.json();
        })
        .then(data => {
            if (data.ajaxStatus == 401) {
                window.location.href = '../Validate/Login'
            }
        })
        //.catch(Error => { )

})