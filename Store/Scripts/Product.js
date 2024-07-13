document.addEventListener('DOMContentLoaded', function () {
    
function AddCart(listID) {
    var data = { productID: listID }
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
            } else {
                alert("購物車添加成功")
                document.getElementById("cartItemsAmounts").textContent = data.CartItemAmounts;
            }
        })
    //.catch(Error => { )
}
//})
});