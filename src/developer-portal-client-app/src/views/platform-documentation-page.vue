<template lang="pug">
  div
    h1 Open Platforms request structure
    h2 Request structure for calls to your platform
    p.
      This documentation describes how to implement
      the integration with Open Platforms in your
      service.
    h2 How it works
    p.
      Briefly described, this is how the Open Platforms
      connection is established with your service.
    ol
      li.
        Create an endpoint (URL) that is publicly
        accessible for calls from Open Platforms to
        request data for a specified user.
        For testing we recommend using #[strong Test mode].
        #[router-link.color-export(to="/platform-settings") Enter your endpoint url] 
        for the platform.
      li.
        The endpoint verifies the security token
        from Open Platforms and returns an HTTP
        status in the 400-599 range (recommended: 401
        Unauthorized) if the token is incorrect.
      li.
        Based on the identifier (the user's e-mail address)
        the endpoint responds with data in the format
        specified by Open Platforms.
      li.
        You test the integration with the test tool
        provided #[router-link.color-export(to="/platform-test") here]
        by Open Platforms.
      li.
        Once the test is successful, request for the
        platform to #[strong Go Live] to be integrated
        in apps chosen by the user.
      li.
        Open Platforms periodically retrieves updates
        from your platform for each connected user.
    h2 Prerequisites
    p.
      The following prerequisites are required for
      implementing the connection with Open Platforms
      and allowing your users to connect their data
      from your service.
    ul
      li.
        Your platform or service is accessible from the
        web and you are able to add a custom url to be
        called by Open Platforms with user data in the
        correct format.
      li.
        You are able to verify a header with a sceurity
        token in the endpoint.
      li.
        Your platform has unique e-mail addresses for
        users so that there is no conflict between which
        account to retrieve user data from.
    h2 JSON Schema
    p.
      Please see the #[router-link.color-export(to="/json-schema") JSON Schema]
      for how to format the response to requests from
      Open Platforms.
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({})
export default class PlatformDocumentationPage extends Vue {}
</script>
