using Contracts.Protos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using NewsService.Data;

namespace NewsService.Feature
{
    public static class NewsConverter
    {
        public static NewsMessage ToProto(this NewsModel model)
        {
            if (model == null) return new NewsMessage();

            return new NewsMessage
            {
                Id = model.Id,
                Titel = model.Titel,
                Tag = model.Tag,
                PreviewText = model.PreviewText,
                MainText = model.MainText,
                CreatedEmail = model.CreatedEmail,

                Image = ByteString.CopyFrom(model.Image ?? Array.Empty<byte>()),

                CreatedAt = Timestamp.FromDateTime(model.CreatedAt.ToUniversalTime())
            };
        }

        public static NewsModel ToModel(this NewsMessage proto)
        {
            if (proto == null) return new NewsModel();

            return new NewsModel
            {
                Id = proto.Id,
                Titel = proto.Titel,
                Tag = proto.Tag,
                PreviewText = proto.PreviewText,
                MainText = proto.MainText,
                CreatedEmail = proto.CreatedEmail,

                Image = proto.Image.ToByteArray(),

                CreatedAt = proto.CreatedAt.ToDateTime()
            };
        }
    }
}
