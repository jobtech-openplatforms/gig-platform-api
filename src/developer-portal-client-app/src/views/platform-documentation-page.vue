<template lang="pug">
  .instructions
    .card
      h1 Open Platforms request structure
      p.
        This documentation describes how to implement
        the integration with Open Platforms in your
        service.
      p Table of contents
      ul.toc
        li #[a.page-nav(@click="goto('#implementation')") Implementation]
        li #[a.page-nav(@click="goto('#howitworks')") How it works]
          ul
            li #[a.page-nav(@click="goto('#your-implementation')") Your implementation]
        li #[a.page-nav( @click="goto('#prerequisites')") Prerequisites]
        li #[a.page-nav( @click="goto('#api')") API]

    .card
      h2#implementation Implementation
      p.
        These are the requirements for integrating
        data from your platform with Open Platforms.
      ul
        li.
          This implementation is primarily for platforms
          without an API or OAuth connections to users
          (gig workers). If your platform already has an
          API for users to connect with and rertieve their
          data on experience, earnings and reputation,
          please get in touch with us about setting updates
          the integration.
        li.
          You will need to build a custom endpoint (URL)
          that receives a security token (to verify that
          the request is from Open Platforms) and an identifier
          for the user.
        li.
          If the identifier is the user's e-mail
          address, Open Platforms will have verified the user's
          ownership of that e-mail address before making
          requests for updates to your platform.

    .card
      h2#howitworks How it works
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


    .card
      h2#prerequisites Prerequisites
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


    .card
      h2#api API
      p.
        Please see the #[router-link.color-export(to="/json-schema") JSON Schema]
        for how to format the response to requests from
        Open Platforms.

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({})
export default class PlatformDocumentationPage extends Vue {
  goto(refName) {
    const el = document.querySelector(refName)
    if (el) el.scrollIntoView({ behavior: 'smooth', block: 'center' })
  }
}
</script>
