const replace = require('replace-in-file');
const config = require('./base-config.json');
const indexConfiguration = process.argv.indexOf('--configuration');
const configuration = indexConfiguration !== -1 ? process.argv[indexConfiguration + 1] : 'dev';

const options = {
  files: ['angular.json'],
  from: /"deployUrl": "(.*)"/g,
  to: `"deployUrl": "${config.deployUrl[configuration]}"`,
  allowEmptyPaths: false
};

try {
  replace.sync(options);
} catch (error) {
  console.error('Error occurred:', error);
}
