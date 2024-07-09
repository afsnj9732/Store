document.getElementById("addCart").addEventListener("click", function () {
    var data = { productID: document.getElementById("addCart").getAttribute("data-productID") }
    url = "../Shopping/AddCart"
    fetch(url, {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    }
    )
        .then(Response => { console.log("ajax成功") })
        .catch(Error => { console.error("ajax失敗") })

})