var vm = new Vue({
    el: '#app',
    data: {
        list: [],
        loading: false,
        dialogVisible: false,
        priceLoading: false,
        priceList: [],
        currentPage: 1,
        total: 0,
        where: {
            Time: [],
            PriceStart: '',
            PriceEnd: '',
            ProductName: ''
        }
    },
    methods: {
        getList: function () {
            var that = this
            that.loading = true
            ajax('/CsProducts/GetCsProductsList', {}, function (data) {
                that.list = data
                that.loading = false
            })
        },
        changeStatus: function (id) {
            var that = this
            that.loading = true
            ajax('/CsProducts/ChangeCsProductsStatus', {
                id: id
            }, function (data) {
                if (data.code === 1) {
                    that.$notify({
                        title: '成功',
                        message: data.data,
                        type: 'success'
                    })
                    that.getList()
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                }
                that.loading = false
            })
        },
        submitPrice: function () {
            var that = this
            var count = 0
            var subModel = []
            that.list.forEach((p) => {
                if (p.ProductPrice <= 0) {
                    count++
                    return false
                }
                subModel.push({
                    ProductId: p.ProductId,
                    ProductPrice: p.ProductPrice
                })
            })
            if (count > 0) {
                that.$notify.error({
                    title: '错误',
                    message: '价格中存在低于或等于0的价格, 请仔细核实价格~'
                })
                return
            }
            that.loading = true
            ajax('/CsPrice/SubmitCsPrice', subModel, function (data) {
                if (data.code === 1) {
                    that.$notify({
                        title: '成功',
                        message: data.data,
                        type: 'success'
                    })
                    that.getList()
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: '系统异常,请刷新后再试'
                    })
                }
                that.loading = false
            })
        },
        getpage: function (page) {
            var that = this
            that.priceLoading = true
            that.where.CurrentPage = page
            if (that.where.Time.length > 1 && that.where.Time[0] != null && that.where.Time[1] != null) {
                that.where.Time = [new Date(that.where.Time[0]).Format('yyyy-MM-dd hh:mm:ss'), new Date(that.where.Time[1]).Format('yyyy-MM-dd hh:mm:ss')]
            }
            ajax('/CsPrice/GetCsPricePage', that.where, function (data) {
                that.priceList = data.data
                that.total = data.total
                that.priceLoading = false
            })
        },
        selectLast: function () {
            this.getpage(this.currentPage)
            this.dialogVisible = true
        }
    },
    mounted: function () {
        this.getList()
    }
})