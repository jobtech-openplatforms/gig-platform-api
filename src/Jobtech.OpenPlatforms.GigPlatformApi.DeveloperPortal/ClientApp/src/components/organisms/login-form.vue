<template lang="pug">
form.login-form( v-on:submit.prevent="handleSubmit")
  h2 Login
  p {{email}}
  p {{password}}
  .form-group
    FormInput(v-model=email type="email" label="E-post" cssClasses="username")
    FormInput(v-model=password value='' type="password" label="Lösenord")
    div( v-if="login.alert.message" :class="`alert ${login.alert.type}`") {{login.alert.message}}
    input.submit.btn-right(type="submit")

    div( v-if="login")
      button.btn-primary.btn-right(v-if="!login.isLoggedIn" @click="loginMutation") Login
      p(v-else) Hello {{ login.user }}
</template>

<script lang="ts">
import FormInput from '@/components/atoms/form-input.vue'
import { Component, Prop, Vue } from 'vue-property-decorator'
import { State, Mutation, namespace } from 'vuex-class'
import { LoginState } from '../../types'

const SessionState = namespace('login', State)
const LoginMutation = namespace('login', Mutation)

@Component({
  components: {
    FormInput,
  },
})
export default class LoginForm extends Vue {
  @State('login') public login: LoginState
  @Prop() public email!: string
  @Prop() public password!: string
  @LoginMutation('login') private loginMutation

  public handleSubmit(ev) {
    window.alert(ev)
  }
}
</script>

<style lang="stylus">
.login-form
  display: block
  border: 1px solid #eee
</style>
