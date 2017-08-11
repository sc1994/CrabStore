var mentApp = new Vue({
    el: '#menuApp',
    data: {
        menuList: []
    },
    methods: {

    },
    mounted: function () {
        var that = this;
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
        menuElement[0].style.height = window.innerHeight - 100 + 'px'
        var thatUrl = document.URL;
    }
})