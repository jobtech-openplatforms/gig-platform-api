<template lang="pug">
  div#app(v-bind:class="{ nologin: !($auth.loading || $auth.isAuthenticated)}")
    TopNav
    //- Loading(:active.sync="loading" :can-cancel="false" :is-full-page="true" loader="dots")
    LoaderOverlay(v-if="loading")
    #main
      SideNav( v-if="!$auth.loading && $auth.isAuthenticated")
      #page
        div#alert-global( v-if="alert.message" :class="`alert ${alert.type}`" v-html="alert.message")
        transition(name="fade" mode="out-in")
          router-view
</template>

<script>
import TopNav from '@/components/molecules/top-nav.vue'
import SideNav from '@/components/molecules/side-nav.vue'
// import Loading from 'vue-loading-overlay'
import LoaderOverlay from '@/components/atoms/loader-overlay.vue'
import { mapState, mapActions, mapGetters } from 'vuex'

export default {
  name: 'app',
  components: {
    TopNav,
    SideNav,
    // Loading,
    LoaderOverlay
  },
  computed: {
    ...mapState({
      alert: (state) => state.alert
    }),
    ...mapState('account', ['status']),
    ...mapGetters('projects', ['loading'])
  },
  methods: {
    ...mapActions({
      clearAlert: 'alert/clear'
    })
  },
  watch: {
    $route(to, from) {
      // clear alert on location change
      this.clearAlert()
    }
  }
}
</script>

<style lang="scss">
@import './assets/scss/general.scss';

#alert-global{
  position:absolute;
  width:100vw;
  text-align:center;
  color:$color-warning;
  background:#fff;
  font-weight:400;
  top:0;
  left:0;
  right:0;
  padding:2rem;
}

#app{
  @include flex(column, null, null);
  min-height:100vh;
}

#main {
  @include flex(row, null, null);
  @include flex-child(1, null, null);
  padding-left:$sidebar-width;
  overflow-x:hidden;
}

#page{
  overflow:hidden;
  @include flex-child(1, null, null);
  max-width:$media-small;
  margin:0 auto;
  padding:4rem 6rem 2rem;
  @include tiny-screen{
    padding:3rem 2rem;
  }

  a{
    font-weight:bold;
    &:not([class^="btn"]){
      color:$color-project;
    }
  }

}

.fade-enter-active,
.fade-leave-active {
  transition-duration: 0.3s;
  transition-property: opacity;
  transition-timing-function: ease;
}

.fade-enter,
.fade-leave-active {
  opacity: 0
}
</style>
