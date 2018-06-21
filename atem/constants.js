"use strict";
const root = require('./helpers.js').root
const ip = require('ip');

const {
  ProvidePlugin
} = require('webpack');
const HtmlElementsWebpackPlugin = require('html-elements-webpack-plugin');

exports.HOST = ip.address();
exports.DEV_PORT = 3000;
exports.E2E_PORT = 4201;
exports.PROD_PORT = 8088;

/**
 * These constants set whether or not you will use proxy for Webpack DevServer
 * For advanced configuration details, go to:
 * https://webpack.github.io/docs/webpack-dev-server.html#proxy
 */
exports.USE_DEV_SERVER_PROXY = false;
exports.DEV_SERVER_PROXY_CONFIG = {
  '**': 'http://localhost:8089'
}

/**
 * These constants set the source maps that will be used on build. 
 * For info on source map options, go to: 
 * https://webpack.github.io/docs/configuration.html#devtool
 */
exports.DEV_SOURCE_MAPS = 'eval';
exports.PROD_SOURCE_MAPS = 'source-map';

/**
 * Set watch options for Dev Server. For better HMR performance, you can 
 * try setting poll to 1000 or as low as 300 and set aggregateTimeout to as low as 0. 
 * These settings will effect CPU usage, so optimal setting will depend on your dev environment.
 * https://github.com/webpack/docs/wiki/webpack-dev-middleware#watchoptionsaggregatetimeout
 */
exports.DEV_SERVER_WATCH_OPTIONS = {
  poll: undefined,
  aggregateTimeout: 300,
  ignored: /node_modules/
}

exports.EXCLUDE_SOURCE_MAPS = [
  // these packages have problems with their sourcemaps
  root('node_modules/@angular'),
  root('node_modules/rxjs')
]

exports.MY_COPY_FOLDERS = [
  // use this for folders you want to be copied in to Client dist
  // src/assets and index.html are already copied by default.
  // format is { from: 'folder_name', to: 'folder_name' }
]

exports.MY_POLYFILL_DLLS = [
  // list polyfills that you want to be included in your dlls files
  // this will speed up initial dev server build and incremental builds.
  // Be sure to run `npm run build:dll` if you make changes to this array.
]

exports.MY_VENDOR_DLLS = [
  // list vendors that you want to be included in your dlls files
  // this will speed up initial dev server build and incremental builds.
  // Be sure to run `npm run build:dll` if you make changes to this array.

  'jquery'
]

exports.MY_CLIENT_PLUGINS = [
  // use this to import your own webpack config Client plugins.

  /*
   * Plugin: HtmlElementsWebpackPlugin
   * Description: Generate html tags based on javascript maps.
   *
   * If a publicPath is set in the webpack output configuration, it will be automatically added to
   * href attributes, you can disable that by adding a "=href": false property.
   * You can also enable it to other attribute by settings "=attName": true.
   *
   * The configuration supplied is map between a location (key) and an element definition object (value)
   * The location (key) is then exported to the template under then htmlElements property in webpack configuration.
   *
   * Example:
   *  Adding this plugin configuration
   *  new HtmlElementsWebpackPlugin({
   *    headTags: { ... }
   *  })
   *
   *  Means we can use it in the template like this:
   *  <%= webpackConfig.htmlElements.headTags %>
   *
   * Dependencies: HtmlWebpackPlugin
   */
  new HtmlElementsWebpackPlugin({
    headTags: require('./head-config')
  }),

  new ProvidePlugin({
    $: "jquery",
    jQuery: "jquery",
    "window.jQuery": "jquery",
  })
]

exports.MY_CLIENT_PRODUCTION_PLUGINS = [
  // use this to import your own webpack config plugins for production use.

  /*
   * Plugin: HtmlElementsWebpackPlugin
   * Description: Generate html tags based on javascript maps.
   *
   * If a publicPath is set in the webpack output configuration, it will be automatically added to
   * href attributes, you can disable that by adding a "=href": false property.
   * You can also enable it to other attribute by settings "=attName": true.
   *
   * The configuration supplied is map between a location (key) and an element definition object (value)
   * The location (key) is then exported to the template under then htmlElements property in webpack configuration.
   *
   * Example:
   *  Adding this plugin configuration
   *  new HtmlElementsWebpackPlugin({
   *    headTags: { ... }
   *  })
   *
   *  Means we can use it in the template like this:
   *  <%= webpackConfig.htmlElements.headTags %>
   *
   * Dependencies: HtmlWebpackPlugin
   */
  new HtmlElementsWebpackPlugin({
    headTags: require('./head-config')
  }),

  new ProvidePlugin({
    $: "jquery",
    jQuery: "jquery",
    "window.jQuery": "jquery",
  })
]

exports.MY_CLIENT_RULES = [
  // use this to import your own rules for Client webpack config.
]

exports.MY_TEST_RULES = [
  // use this to import your own rules for Test webpack config.
]

exports.MY_TEST_PLUGINS = [
  // use this to import your own Test webpack config plugins.
]
