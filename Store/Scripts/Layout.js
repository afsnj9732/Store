
grecaptcha.ready(function () {
    grecaptcha.execute('6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN').then(function (token) {
        document.getElementById('recaptcha').value = token;
    });
});
function GetCartItemQuantity() {
    url = "../Shopping/GetCartItemQuantity"
    fetch(url,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json' 
            },
        }
    )
        .then(Response => {
            return Response.json();
        })
        .then(data => {
            if (data.ajaxStatus!=401) {
                document.getElementById("cartItemsQuantity").textContent = data.CartItemQuantity;
            }

        })
}
function AddCart(listID) {
    var data = { productID: listID }
    url = "../Shopping/AddCart"
    fetch(url,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
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
                GetCartItemQuantity();

            }
        })

}
document.addEventListener('DOMContentLoaded', function () {
    GetCartItemQuantity();
});
