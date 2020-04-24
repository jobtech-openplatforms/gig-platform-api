<template lang="pug">

  .top-nav
    div(class="app-logo-container")
      <img class="app-logo" src="../../assets/img/open-platforms-logo-text.svg">
    //- router-link.btn.color-export( to="/platform-settings" active-class="active") Platform API documentation//
    //- router-link.btn.color-import( to="/application-settings" active-class="active") Application API documentation//
    div(v-if="!$auth.loading")
      router-link.btn.color-project( to="/project-start" active-class="active") Help

      .btn( v-if="$auth.isAuthenticated" @click="logout") Log out
      .btn( v-if="$auth.isAuthenticated == false" @click="login") Log in

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions } from 'vuex'

@Component({
  computed: {
    ...mapState('account', ['status', 'user'])
  },
  methods: {
    // Log the user in
    login() {
      console.log('login click!')
      this.$auth.loginWithRedirect()
    },
    // Log the user out
    logout() {
      this.$auth.logout({
        returnTo: window.location.origin
      })
    }
  }
})
export default class TopNav extends Vue {}
</script>

<style lang="scss">
.app-logo-container {
  height:8vw;
  max-height: 40px;
  img {
    width: auto;
    height: 100%;
  }
}
.top-nav {
  @include flex(row, space-between, center);
  height: $topbar-height;
  background: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.25);
  overflow: hidden;
  padding: 1rem;
  position: sticky;
  top: 0;
  z-index: 3;
  a {
    color: initial;
  }

  h4 {
    margin-bottom: 0;
  }
}
</style>
