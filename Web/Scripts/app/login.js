var vm = new Vue({
    el: '#app',
    data: {
        loginForm: {
            account: '',
            password: '',
            remember: true
        },
        loading: false,
        rules: {
            account: [{
                    required: true,
                    message: '请输入账号',
                    trigger: 'blur'
                },
                {
                    min: 6,
                    max: 18,
                    message: '长度在 6 到 18 个字符',
                    trigger: 'blur'
                }
            ],
            password: [{
                    required: true,
                    message: '请输入密码',
                    trigger: 'blur'
                },
                {
                    min: 6,
                    max: 18,
                    message: '长度在 6 到 18 个字符',
                    trigger: 'blur'
                }
            ]
        }
    },
    methods: {
        login: function () {
            var that = this
            that.loading = true
            that.loginForm.password = md5(that.loginForm.password)
            ajax('/Login/Login', that.loginForm, function (data) {
                if (data.code === 1) {
                    that.$notify({
                        title: ' ',
                        message: data.data,
                        type: 'success'
                    });
                    setTimeout(function () {
                        window.location.href = '/CsPrice'
                    }, 300)
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                    that.loginForm.password = ''
                    that.loading = false
                }
            });
        }
    }
})