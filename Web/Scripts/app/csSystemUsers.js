var vm = new Vue({
    el: '#app',
    data: {
        where: {
            SysUserName: '',
            SysUserState: '1',
            SysUserType: ''
        },
        list: [],
        currentPage: 1,
        loading: false,
        total: 0,
        dialogVisible: false,
        dialogLoading: false,
        info: {
            SysUserName: '',
            SysUserPassword: '',
            SysUserType: '2',
            SysUserState: '1',
            DeleteDate: '',
            DeleteDescribe: '',
            SysUserId: 0
        },
        oldInfo: {
            SysUserName: '',
            SysUserPassword: '',
            SysUserType: '2',
            SysUserState: '1',
            DeleteDate: '',
            DeleteDescribe: '',
            SysUserId: 0
        },
        rules: {
            SysUserName: [{
                required: true,
                message: '请输入用户名',
                trigger: 'blur'
            }, {
                min: 6,
                max: 18,
                message: '长度在 6 到 18 个字符',
                trigger: 'blur'
            }],
            SysUserPassword: [{
                required: true,
                message: '请输入密码',
                trigger: 'blur'
            }, {
                min: 6,
                max: 18,
                message: '长度在 6 到 18 个字符',
                trigger: 'blur'
            }]
        }
    },
    methods: {
        getpage: function (page) {
            var that = this;
            that.loading = true
            that.where.CurrentPage = page
            ajax('/CsSystemUsers/GetCsSystemUsersPage', that.where, function (data) {
                that.list = data.data
                that.total = data.total
                that.loading = false
            })
        },
        add: function () {
            this.info.SysUserId = 0
            this.dialogVisible = true
            this.info = {
                SysUserName: '',
                SysUserPassword: '',
                SysUserType: '2',
                SysUserState: '1',
                DeleteDate: '',
                DeleteDescribe: '',
                SysUserId: 0
            }
            this.oldInfo = {
                SysUserName: '',
                SysUserPassword: '',
                SysUserType: '2',
                SysUserState: '1',
                DeleteDate: '',
                DeleteDescribe: '',
                SysUserId: 0
            }
        },
        getInfo: function (id) {
            var that = this
            that.info.SysUserId = id
            that.loading = true
            ajax('/CsSystemUsers/GetCsSystemUsersModel', {
                id: id
            }, function (data) {
                that.loading = false
                that.dialogVisible = true
                if (data.code === 1) {
                    that.info = data.data
                    that.oldInfo = {
                        SysUserName: data.data.SysUserName,
                        SysUserPassword: data.data.SysUserPassword,
                        SysUserType: data.data.SysUserType,
                        SysUserState: data.data.SysUserState,
                        DeleteDate: data.data.DeleteDate,
                        DeleteDescribe: data.data.DeleteDescribe,
                        SysUserId: data.data.SysUserId
                    }
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
            if (!that.info.SysUserName || !that.info.SysUserPassword) {
                that.$notify({
                    title: '请注意',
                    message: "请填写必要信息",
                    type: 'error'
                })
                return;
            }
            that.dialogLoading = true
            if (that.oldInfo.SysUserPassword !== that.info.SysUserPassword) {
                that.info.SysUserPassword = md5(that.info.SysUserPassword)
            }
            ajax('/CsSystemUsers/SubmitCsSystemUsers', that.info, function (data) {
                if (data.code === 1) {
                    that.$notify({
                        title: '操作成功',
                        message: ' ',
                        type: 'success'
                    });
                    that.dialogVisible = false
                    that.getpage(that.currentPage)
                } else if (data.code === 2) {
                    that.$notify({
                        title: '请注意',
                        message: data.data,
                        type: 'warning'
                    })
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: '网络异常, 请稍后重试'
                    })
                }
                that.dialogLoading = false
            })
        }
    },
    watch: {
        'info.SysUserState': function (newValue) {
            if (newValue == 0) {
                if (this.info.DeleteDate == '1900-1-1 12:00:00')
                    this.info.DeleteDate = new Date().Format('yyyy-M-d hh:mm:ss')
            }
        }
    },
    mounted: function () {
        this.getpage(this.currentPage)
    }
})