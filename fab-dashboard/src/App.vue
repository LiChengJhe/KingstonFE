<template>
  <div id="app"  >
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
      <router-link class="navbar-brand" to="/">
        <font-awesome-icon icon="laptop-house" class="mr-1" />CIC
      </router-link>
      <button
        class="navbar-toggler"
        type="button"
        data-toggle="collapse"
        data-target="#navbarNavAltMarkup"
        aria-controls="navbarNavAltMarkup"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
        <div class="navbar-nav">
          <router-link class="nav-link" to="/dashboard">
            <font-awesome-icon icon="laptop" class="mr-1" />Dashboard
          </router-link>
          <router-link class="nav-link" to="/report">
            <font-awesome-icon icon="chart-bar" class="mr-1" />Report
          </router-link>
        </div>
      </div>
    </nav>

    <router-view />
  </div>
</template>
