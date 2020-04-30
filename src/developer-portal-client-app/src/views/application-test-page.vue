<template lang="pug">
  div.test-application-page

    h1 Test your application
    p.
      To help you test the application you are developing to retrieve data from
      platforms for a user, we have created some test methods you can use to
      send test data to receive in your application.
    p.
      #[strong Tip!] #[em To test while developing on localhost, consider using a service like] #[a(href="https://ngrok.com/" target="_blank") ngrok] #[em to receive data from Open Platforms to your development machine.]
    current-application-tokens
    hr
    .card
      h2 Available test functions
      ol
        li
          strong Authenticate user with Open Platforms
        li
          strong Receive user's data from Open Platforms
    h2 Authenticate user with Open Platforms
    p.
      When a user connection through Open Platforms is
      successful, your server will receive a callback
      with #[code result=completed], the #[code requestid]
      you sent when initiating the connection,
      and the #[code openPlatformsUserId] of the connected user.
    p.
      Test your application response with #[code result=completed]
    app-auth-test(result="completed" buttonText="Try it")
    p.
      Test your application response with #[code result=failed]
    app-auth-test(result="failed" buttonText="Try it")

    hr

    h2 Receive user's data from Open Platforms
    p.
      A request with the data for a single connection
      fo a user will be sent to the data url you have
      specified for the project.
    p.
      See the #[router-link(to="/application-documentation") full documentation]
      for detailed instructions.

    div(v-if="currentApplication && currentApplication.dataUpdateCallbackUrl")
      p Data update url for this application:&nbsp;
        code {{currentApplication.dataUpdateCallbackUrl}}
    app-auth-test(buttonText="Try it")
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import CurrentApplicationTokens from '../components/organisms/current-application-tokens.vue'
import AppAuthTest from '@/components/organisms/app-auth-test.vue'
import AppDataTest from '@/components/organisms/app-data-test.vue'
import { mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapGetters('projects', ['currentApplication'])
  },
  components: {
    CurrentApplicationTokens,
    AppAuthTest,
    AppDataTest
  }
})
export default class TestApplicationPage extends Vue {}
</script>

