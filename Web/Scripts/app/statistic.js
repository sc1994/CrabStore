var vm = new Vue({
    el: '#app',
    data: {
        where: {
            ProductType: '',
            ProductName: '',
            PartType: ''
        },
        loading: false,
        list: [],
        time: ''
    },
    methods: {
        getList: function (isUnLoad) {
            var that = this
            if (!isUnLoad) {
                that.loading = true
            }
            ajax('/CsOrder/GetStatisticList', that.where, function (data) {
                that.list = data
                if (!isUnLoad) {
                    that.loading = false
                }
            })
        }
    },
    mounted: function () {
        var that = this
        that.getList()
        that.time = new Date().Format('yyyy-MM-dd hh:mm')
        setInterval(function () {
            that.time = new Date().Format('yyyy-MM-dd hh:mm')
            that.getList(true)
        }, 15000);
    }
})