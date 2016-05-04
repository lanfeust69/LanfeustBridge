/* global require, module */

var Angular2App = require('angular-cli/lib/broccoli/angular2-app');

module.exports = function(defaults) {
  var app = new Angular2App(defaults, {
    vendorNpmFiles: [
      'es6-shim/es6-shim.js',
      'systemjs/dist/system-polyfills.js',
      'angular2/bundles/angular2-polyfills.js',
      'systemjs/dist/system.src.js',
      'rxjs/bundles/Rx.js',
      'angular2/bundles/angular2.dev.js',
      'angular2/bundles/http.dev.js',
      'angular2/bundles/router.dev.js',
      'ng2-bootstrap/bundles/ng2-bootstrap.min.js',
      'moment/moment.js',
      'bootstrap/3.3.6/css/bootstrap.min.css'
    ]
  });
  return app.toTree();
};
