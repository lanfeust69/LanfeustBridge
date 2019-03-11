/*global jasmine */
var SpecReporter = require('jasmine-spec-reporter').SpecReporter;
var JUnitXmlReporter = require('jasmine-reporters').JUnitXmlReporter;

exports.config = {
  allScriptsTimeout: 11000,
  specs: [
    'e2e/**/*.e2e.ts'
  ],
  capabilities: {
    browserName: 'chrome',
    loggingPrefs: { "driver": "INFO", "browser": "INFO" }
  },
  directConnect: true,
  baseUrl: 'http://localhost:5000/',
  framework: 'jasmine',
  jasmineNodeOpts: {
    showColors: true,
    defaultTimeoutInterval: 3000000,
    print: function() {}
  },
  onPrepare: function() {
    require('ts-node').register({
      project: 'e2e/tsconfig.e2e.json'
    });
    jasmine.getEnv().addReporter(new SpecReporter());
    jasmine.getEnv().addReporter(new JUnitXmlReporter({ savePath: 'junit/' }));
  }
};
