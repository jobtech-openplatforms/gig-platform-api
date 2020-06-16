<template lang="pug">
  div#home
    #start-upper
      #start-title
        h1.color-import Open Platforms developer portal
        p.start-sub Be part of our digital infrastructure enabling portability of data in the gig economy!
        //- p The open platforms API makes it possible to:


    #start-info
      strong Connect your platform
      p.
        Let your user share their experience and reputation
        from your platform by integrating with the
        #[strong.color-export Platform API].
      strong Register your application
      p.
        Let your user access their data from Open Platforms partners by
        connecting to the API. Read the
        #[a.color-import(href="http://gig-data-api-api-open-platforms.test.services.jtech.se/index.html" title="Read the documentation for the application API" target="_blank")  Application API docs].
      button.btn.my-4.btn-huge.btn-project( v-if="$auth.isAuthenticated == false" @click="login") Get started!

 </template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions } from 'vuex'
import { getInstance } from '../auth/auth-wrapper'

export default {
  data() {
    return {
      user: {
        name: '',
        email: '',
        password: ''
      },
      submitted: false
    }
  },
  computed: {
    ...mapState('account', ['status']),
    ...mapState('projects', ['all'])
    // ...mapActions('account', ['register']),
  },
  methods: {
    // Log the user in
    login() {
      this.$auth.loginWithRedirect()
    }
  },
  async created() {
    const authService = getInstance()
    const fn = () => {
      // If the user is authenticated, continue with the route
      if (authService.isAuthenticated && this.all) {
        this.$router.push('/projects')
      } else if (authService.isAuthenticated) {
        this.$router.push('/create')
      }
    }
    authService.$watch('loading', (loading) => {
      if (loading === false) {
        return fn()
      }
    })
  }
}
</script>


<style lang="scss">
.nologin #main {
  background:#3b3b3b;
  max-width:none;
  @include small-screen-and-up {
    background: #3b3b3b url('../assets/img/open-platforms-logo-icon.svg') right center /
      50vw auto no-repeat;
  }
  @include small-screen{
    background-size: 40vw;
    background-position:right 20%;
  }
  display: flex;
  padding: 0;
  position: relative;

  #page {
    width: auto;
    max-width: none;
    z-index: 1;
    padding-left:6vw;
    padding-right:6vw;

    #home{
      display:block;
    }
  }
}
#home {
  color: #fff;
  @include flex(column, null, null);
  min-height: 100%;

  #start-upper {
    @include flex(row, space-between, flex-start);
    flex: 1 0 auto;
    #start-title {
      flex: 0 1 auto;

      h1 {
        font-size: 3rem;
      }
      .start-sub {
        font-size: 2rem;
      }
    }
    @include small-screen-and-up {
      align-items:center;
      #start-title {
        max-width: 56vw;

        h1 {
          font-size: 3.2vw;
        }
        .start-sub{
          font-size: 2.4vw;
          line-height: 1.2;
          margin-top:8rem;
          margin-bottom:8rem;
        }
      }
    }
    #sign-up {
      flex: 0 1 400px;
      margin: $space-big;

      form.card {
        background-color: $color-project;
        padding: 2rem 3rem;
        .btn {
          padding: 0;
        }
      }
    }
  }
  #start-info {
    flex: 1 0 auto;
    width: 40%;
    min-width:320px;

    @include tiny-screen{
    width: 100%;
    justify-content: flex-end;
    display: flex;
    flex-direction: column;
    }

    a {
      font-weight: bold;
      margin: 0 2px;
      white-space: nowrap;
    }
  }
}
</style>
