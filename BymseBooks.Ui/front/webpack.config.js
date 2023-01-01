const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = {
  entry: "./index.js",
  plugins: [
    new HtmlWebpackPlugin({
      publicPath: "/front/build/",
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
    path: path.resolve(__dirname, "build"),
    clean: true,
  },
};