<template lang="pug">
  div
    h1 Open Platforms request structure
    h2 Request structure for calls to your application
    p.
      This documentation describes how to implement
      the integration with Open Platforms in your
      application.
    h2 How it works
    p.
      Briefly described, this is how the Open Platforms
      connection is established with your service.
    ol
      li.
        Create three endpoints (URLs) that are publicly
        accessible for calls from Open Platforms to
        update data for a specified user.
        For testing we recommend using #[strong Test mode].
        #[router-link.color-export(to="/application-settings") Enter your endpoint url] 
        for the application.
      li.
        The endpoints should verifiy the application ID and Secret Key
        from Open Platforms and return an HTTP
        status in the 400-599 range (recommended: 401
        Unauthorized) if the token is incorrect.
      li.
        Based on the identifier (the user's e-mail address)
        the endpoint responds with data in the format
        specified by Open Platforms.
      li.
        You test the integration with the test tool
        provided #[router-link.color-export(to="/application-test") here]
        by Open Platforms.
      li.
        Once the test is successful, request for the
        application to #[strong Go Live] to be integrated
        in apps chosen by the user.
      li.
        Open Platforms periodically sends updates
        from your application for each connected user.
    h2 Prerequisites
    p.
      The following prerequisites are required for
      implementing the connection with Open Platforms
      and allowing your users to connect their data
      from your service.
    ul
      li.
        Your application or service is accessible from the
        web and you are able to add custom urls to be
        called by Open Platforms with user data in the
        correct format.
      li.
        You are able to verify a header with authentication
        in the endpoint.
    h2 JSON Schema
    p.
      #[strong TODO:] Describe the format for communication
      with applications
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({})
export default class ApplicationDocumentationPage extends Vue {}
</script>
