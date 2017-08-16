var vm = new Vue({
    el: "#app",
    data: {
        where: {
            PartName: '',
            PartType: '',
            PartState: '1'
        },
        list: [],
        loading: false,
        info: {
            PartId: 0,
            PartName: '',
            PartPrice: '',
            PartWeight: '',
            PartType: '',
            OperationDate: '',
            PartState: ''
        },
        dialogVisible: false
    },
    methods: {
        getList: function () {
            var that = this
            ajax('/CsParts/GetCsPartsList', that.where, function (data) {
                that.list = data.data
            })
        },
        getInfo: function (id) {
            var that = this
            that.loading = true
            ajax('/CsParts/GetCsPartsModel', {
                id: id
            }, function (data) {
                that.loading = false
                if (data.code === 1) {
                    that.info = data.data
                    that.dialogVisible = true
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                }
            })
        },
        submitInfo: function () {
            var that = this
            if (that.info.PartWeight <= 0) {
                that.$notify.error({
                    title: '错误',
                    message: '重量不能小于等于0'
                })
                return
            }
            if (that.info.PartPrice <= 0) {
                that.$notify.error({
                    title: '错误',
                    message: '结果不能小于等于0'
                })
                return
            }
            ajax('/CsParts/SubmitCsParts', that.info, function (data) {
                if (data.code === 1) {
                    that.$notify({
                        title: '更新成功',
                        message: ' ',
                        type: 'success'
                    })
                    that.dialogVisible = false
                    that.getList()
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                }
            })
        }
    },
    mounted: function () {
        this.getList()
    }
})