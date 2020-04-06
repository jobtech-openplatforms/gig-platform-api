<template lang="pug">
  .instructions
    h4 What's needed?
    ol
      li
        p You need to build an endpoint that returns the user's data in our specified data format. You can read more about the #[router-link.color-export(to="/platform-api-requests") request structure and data format here]
      li
        p When a user wants to access their data, we will make a request to your endpoint on their behalf. This request will include two parameters:
        ul
          li #[strong PlatformToken] - this is used to identify that the request is coming from our servers. This token should be kept secret and unique for your platform.
          li
            p #[strong UserEmail] - This is the email the user has registered on your platform. Open platforms will confirm that the user has control over this address.
      p #[strong PLEASE NOTE] We are aware that this is a very simple authentification concept, used to keep things as simple as possible. If you do have an Oauth2-based authentification solution on your platform, please contact us and we will help you connect that way instead.

</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({

})
</script>
