﻿namespace BinaryArchive00.Tests;

public class ArchivesFixture : IDisposable
{
    public ArchiveFile[] Archives { get; }
    public ArchiveFile Dv2Ca { get; }
    public ArchiveFile Dv2PatchCa { get; }
    public ArchiveFile LanguageCa { get; }
    public ArchiveFile LangPatchCa { get; }
    public ArchiveFile MusicCa { get; }
    public ArchiveFile Music2Ca { get; }
    public ArchiveFile GiantglenCa { get; }
    public ArchiveFile TestProfileCa { get; }

    public ArchivesFixture()
    {
        var caPaths = Directory.GetFiles(Config.PathToArchives, "*.ca", SearchOption.AllDirectories).ToList();
        var customCaPaths = Directory.GetFiles("CustomArchives", "*.ca", SearchOption.AllDirectories);
        foreach (var customCaPath in customCaPaths)
        {
            var caPath = caPaths.FirstOrDefault(caPath => Path.GetFileName(caPath) == Path.GetFileName(customCaPath));
            if (caPath is null)
                caPaths.Add(customCaPath);
        }

        Archives = caPaths.Select(caPath => ArchiveFile.Open(caPath, readEntriesContent: false)).ToArray();

        Dv2Ca = Archives.Single(archive => archive.FileName == "dv2.ca");
        Dv2PatchCa = Archives.Single(archive => archive.FileName == "dv2patch.ca");
        MusicCa = Archives.Single(archive => archive.FileName == "music.ca");
        Music2Ca = Archives.Single(archive => archive.FileName == "music2.ca");
        LanguageCa = Archives.Single(archive => archive.FileName == "language.ca");
        LangPatchCa = Archives.Single(archive => archive.FileName == "langpatch.ca");
        GiantglenCa = Archives.Single(archive => archive.FileName == "giantglen.ca");
        TestProfileCa = Archives.Single(archive => archive.FileName == "TestProfile.ca");
    }

    public void Dispose()
    {
        foreach (var archive in Archives)
            archive.Dispose();
        GC.SuppressFinalize(this);
    }
}
