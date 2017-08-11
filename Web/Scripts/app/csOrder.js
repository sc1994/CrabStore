var vm = new Vue({
    el: '#app',
    data: {
        where: {
            RowStatus: '1',
            Time: [],
            Status: ''
        },
        currentPage: 1,
        list: [],
        total: 0,
        loading: false
    },
    methods: {
        getpage: function (currentPage) {
            var that = this;
            that.where.CurrentPage = currentPage
            if (that.where.Time.length > 1 && that.where.Time[0] != null && that.where.Time[1] != null) {
                that.where.Time = [new Date(that.where.Time[0]).Format('yyyy-MM-dd'), new Date(that.where.Time[1]).Format('yyyy-MM-dd')]
            }
            that.loading = true
            ajax('/CsOrder/GetCsOrderPage', that.where, function (data) {
                that.list = data.data
                that.currentPage = currentPage
                that.total = data.total
                that.loading = false
            })
        },
        expandRow: function (row) {
            console.log(row)
        }
    },
    mounted: function () {
        this.getpage(1)
    }
})