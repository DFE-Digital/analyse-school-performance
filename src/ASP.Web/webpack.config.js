const path = require('path')
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("copy-webpack-plugin");

const stylesConfig = {
    mode: process.env.NODE_ENV,
    entry: {
        govuk: path.resolve(__dirname, 'scripts/govuk_scss.js'),
        app: path.resolve(__dirname, 'scripts/app_scss.js'),
        components: path.resolve(__dirname, 'scripts/components_scss.js'),
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot'),    
    },
    module: {
        rules: [
            {
                test: /\.(css|scss)$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            url: false
                        }
                    },
                    {
                        loader: 'postcss-loader'
                    },
                    {
                        loader: 'sass-loader',
                    }
                ]
            }
        ],
    },
    plugins: [
        new CopyPlugin({
            patterns: [
                { from: "node_modules/govuk-frontend/dist/govuk/assets", to: "assets" },
                { from: "node_modules/govuk-frontend/dist/govuk/assets/images/favicon.ico", to: "" },
            ],
        }),
        new MiniCssExtractPlugin({
            filename: 'assets/css/[name].css'
        })
    ]
};

const javaScriptConfig = {
    mode: process.env.NODE_ENV,
    entry: {
        govuk: path.resolve(__dirname, 'scripts/govuk.js'),
        alpine: path.resolve(__dirname, 'scripts/alpine.js'),
        components: path.resolve(__dirname, 'scripts/components.js'),
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/assets/js'),
        filename: '[name].js',
    }
};

module.exports = [stylesConfig, javaScriptConfig];
