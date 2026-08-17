using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.ComWebHelper
{

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    //
    // 摘要:
    //     Specifies the media type information for an email message attachment.

    public static class CustomMediaTypeNames

    {
        //
        // 摘要:
        //     Specifies the kind of application data in an email message attachment.
        public static class Application
        {
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in URL
            //     encoded format.
            public const string FormUrlEncoded = "application/x-www-form-urlencoded";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in JSON
            //     format.
            public const string Json = "application/json";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in JSON
            //     patch format.
            public const string JsonPatch = "application/json-patch+json";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in JSON
            //     text sequence format.
            public const string JsonSequence = "application/json-seq";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in Web
            //     Application Manifest.
            public const string Manifest = "application/manifest+json";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is not interpreted.
            public const string Octet = "application/octet-stream";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in Portable
            //     Document Format (PDF).
            public const string Pdf = "application/pdf";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in JSON
            //     problem detail format.
            public const string ProblemJson = "application/problem+json";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in XML
            //     problem detail format.
            public const string ProblemXml = "application/problem+xml";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in Rich
            //     Text Format (RTF).
            public const string Rtf = "application/rtf";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is a SOAP
            //     document.
            public const string Soap = "application/soap+xml";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in WASM
            //     format.
            public const string Wasm = "application/wasm";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in XML
            //     format.
            public const string Xml = "application/xml";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in XML
            //     Document Type Definition format.
            public const string XmlDtd = "application/xml-dtd";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is in XML
            //     patch format.
            public const string XmlPatch = "application/xml-patch+xml";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Application data is compressed.
            public const string Zip = "application/zip";
        }
        //
        // 摘要:
        //     Specifies the kind of font data in an email message attachment.
        public static class Font
        {
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Font data is in font type collection
            //     format.
            public const string Collection = "font/collection";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Font data is in OpenType Layout
            //     (OTF) format.
            public const string Otf = "font/otf";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Font data is in SFNT format.
            public const string Sfnt = "font/sfnt";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Font data is in TrueType font
            //     (TTF) format.
            public const string Ttf = "font/ttf";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Font data is in WOFF format.
            public const string Woff = "font/woff";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Font data is in WOFF2 format.
            public const string Woff2 = "font/woff2";
        }
        //
        // 摘要:
        //     Specifies the type of image data in an email message attachment.
        public static class Image
        {
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in AVIF format.
            public const string Avif = "image/avif";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in BMP format.
            public const string Bmp = "image/bmp";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in Graphics Interchange
            //     Format (GIF).
            public const string Gif = "image/gif";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in ICO format.
            public const string Icon = "image/x-icon";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in Joint Photographic
            //     Experts Group (JPEG) format.
            public const string Jpeg = "image/jpeg";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in PNG format.
            public const string Png = "image/png";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in SVG format.
            public const string Svg = "image/svg+xml";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in Tagged Image
            //     File Format (TIFF).
            public const string Tiff = "image/tiff";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Image data is in WEBP format.
            public const string Webp = "image/webp";
        }
        //
        // 摘要:
        //     Specifies the kind of multipart data in an email message attachment.
        public static class Multipart
        {
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Multipart data consists of
            //     multiple byte ranges.
            public const string ByteRanges = "multipart/byteranges";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Multipart data is in format.
            public const string FormData = "multipart/form-data";
        }
        //
        // 摘要:
        //     Specifies the type of text data in an email message attachment.
        public static class Text
        {
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in CSS format.
            public const string Css = "text/css";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in CSV format.
            public const string Csv = "text/csv";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in HTML format.
            public const string Html = "text/html";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in Javascript
            //     format.
            public const string JavaScript = "text/javascript";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in Markdown format.
            public const string Markdown = "text/markdown";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in plain text
            //     format.
            public const string Plain = "text/plain";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in Rich Text Format
            //     (RTF).
            public const string RichText = "text/richtext";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in Rich Text Format
            //     (RTF).
            public const string Rtf = "text/rtf";
            //
            // 摘要:
            //     Specifies that the System.Net.Mime.MediaTypeNames.Text data is in XML format.
            public const string Xml = "text/xml";
        }
    }
}
