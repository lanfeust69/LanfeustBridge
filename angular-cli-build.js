/* global require, module */

var Angular2App = require('angular-cli/lib/broccoli/angular2-app');

module.exports = function(defaults) {
  var app = new Angular2App(defaults, {
    vendorNpmFiles: [
      'ng2-bootstrap/bundles/ng2-bootstrap.min.js',
      'moment/moment.js'
    ]
  });
  return app.toTree();
};
