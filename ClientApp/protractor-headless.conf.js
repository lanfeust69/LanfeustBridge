var config = require('./protractor.conf').config

config.capabilities.chromeOptions = { args: [ "--headless" ] }

exports.config = config
