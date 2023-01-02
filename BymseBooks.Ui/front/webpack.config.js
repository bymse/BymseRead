const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = function (outDir) {
  return {
    context: __dirname,
    entry: "./index.js",
    plugins: [
      new HtmlWebpackPlugin({
        publicPath: "/",
        template: "./index.html",
      }),
    ],
    module: {
      rules: [
        {
          test: /\.s[ac]ss$/i,
          use: [
            // Creates `style` nodes from JS strings
            "style-loader",
            // Translates CSS into CommonJS
            "css-loader",
            // Compiles Sass to CSS
            "sass-loader",
          ],
        },
      ]
    },
    output: {
      filename: "main.js",
      path: outDir,
    },
    resolve: {
      modules: [
        'node_modules/bymse-books-front/node_modules'
      ],
    },
  }
};