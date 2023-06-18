const path = require("path");
const config = require("../../BymseRead.Ui/front/webpack.config");

module.exports = env => {
  let out;
  if (env.output) {
    out = path.resolve(__dirname, "..", env.output);
  } else {
    out = path.resolve(__dirname, "..", "wwwroot");
  }
  
  return config(out);
}