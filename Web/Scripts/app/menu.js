var mentApp = new Vue({
    el: '#menuApp',
    data: {
        menuList: [],
        defaultActive: "0-1"
    },
    methods: {
        routeTo: function (item) {
            window.location.href = item.url
        }
    },
    mounted: function () {
        var that = this
        var thatUrl = document.URL
        ajax('/Home/Menu', {}, function (data) {
            data.forEach((m) => {
                if (m.child.length > 0) {
                    m.child.forEach((c) => {
                        if (thatUrl.indexOf(m.url) > -1) {
                            that.$children[0].$data.activedIndex = c.id
                            return false
                        }
                    })
                }
                if (thatUrl.indexOf(m.url) > -1) {
                    that.$children[0].$data.activedIndex = m.id
                    return false
                }
            });
            that.menuList = data
        })
        var menuElement = document.getElementsByClassName('el-menu-vertical')
        menuElement[0].style.height = document.body.scrollHeight - 90 + 'px'
        setInterval(function () {
            if (document.body.scrollHeight - 90 + "px" != menuElement[0].style.height)
                menuElement[0].style.height = document.body.scrollHeight - 90 + 'px'
        }, 300)

    }
})

function signout(userName) {
    ajax('/Login/LogionOut', {
        userName: userName
    }, function (data) {
        if (data.code === 1) {
            window.location.href = '/Login'
        }
    })
}