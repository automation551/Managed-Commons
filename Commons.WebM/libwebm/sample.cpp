// Copyright (c) 2010 The WebM project authors. All Rights Reserved.
//
// Use of this source code is governed by a BSD-style license and patent
// grant that can be found in the LICENSE file in the root of the source
// tree. All contributing project authors may be found in the AUTHORS
// file in the root of the source tree.
//
// This sample application is designed to help clients understand how to use
// mkvparser library.
// By using the mkvparser lib clients are able to handle a matroska format file.

#include "mkvreader.hpp"
#include "mkvparser.hpp"

static const wchar_t* utf8towcs(const char* str)
{
    if (str == NULL)
        return NULL;
        
    //TODO: this probably requires that the locale be 
    //configured somehow:
    
    const size_t size = mbstowcs(NULL, str, 0);
       
    if (size == 0)
        return NULL;
        
    wchar_t* const val = new wchar_t[size+1];
    
    mbstowcs(val, str, size);
    val[size] = L'\0';
    
    return val;
}


int main(int argc, char* argv[])
{
    if (argc == 1) 
    { 
        printf("\t\t\tMkv Parser Sample Application\n");
        printf("\t\t\tUsage: \n");
        printf("\t\t\t  ./sample [input file] \n");
        printf("\t\t\t  ./sample sample.mkv \n"); 
        return -1;
    }
  
    using namespace mkvparser;
 
    MkvReader reader;
  
    if (reader.Open(argv[1]))
    { 
        printf("\n Filename is invalid or error while opening.\n");
        return -1;
    }

    long long pos = 0;

    EBMLHeader ebmlHeader;

    ebmlHeader.Parse(&reader, pos);

    printf("\t\t\t    EBML Header\n"); 
    printf("\t\tEBML Version\t\t: %lld\n", ebmlHeader.m_version); 
    printf("\t\tEBML MaxIDLength\t: %lld\n", ebmlHeader.m_maxIdLength);
    printf("\t\tEBML MaxSizeLength\t: %lld\n", ebmlHeader.m_maxSizeLength);
    printf("\t\tDoc Type\t\t: %s\n", ebmlHeader.m_docType);
    printf("\t\tPos\t\t\t: %lld\n", pos);  
 
    mkvparser::Segment* pSegment;
    
    long long ret = mkvparser::Segment::CreateInstance(&reader, pos, pSegment);
    if (ret) 
    {
        printf("\n Segment::CreateInstance() failed."); 
        return -1;
    }

    ret  = pSegment->Load();
    if (ret < 0) 
    {
        printf("\n Segment::Load() failed."); 
        return -1;
    } 
 
    const SegmentInfo* const pSegmentInfo = pSegment->GetInfo();
   
    const long long timeCodeScale = pSegmentInfo->GetTimeCodeScale();
    const long long duration_ns = pSegmentInfo->GetDuration();
    const wchar_t* const pTitle = utf8towcs(pSegmentInfo->GetTitleAsUTF8());
    const wchar_t* const pMuxingApp = utf8towcs(pSegmentInfo->GetMuxingAppAsUTF8());
    const wchar_t* const pWritingApp = utf8towcs(pSegmentInfo->GetWritingAppAsUTF8());

    printf("\n");
    printf("\t\t\t   Segment Info\n");
    printf("\t\tTimeCodeScale\t\t: %lld \n", timeCodeScale);
    printf("\t\tDuration\t\t: %lld\n", duration_ns);
    
    const double duration_sec = double(duration_ns) / 1000000000;
    printf("\t\tDuration(secs)\t\t: %7.3f\n", duration_sec);

    if (pTitle == NULL)
        printf("\t\tTrack Name\t\t: NULL\n");
    else
        printf("\t\tTrack Name\t\t: %ls\n", pTitle);
   
    if (pMuxingApp == NULL)
        printf("\t\tMuxing App\t\t: NULL\n");
    else 
        printf("\t\tMuxing App\t\t: %ls\n", pMuxingApp);

    if (pWritingApp == NULL) 
        printf("\t\tWriting App\t\t: NULL\n");
    else 
        printf("\t\tWriting App\t\t: %ls\n", pWritingApp);

    printf("\t\tPosition(Segment)\t: %lld\n", pSegment->m_start); // position of segment payload 
    printf("\t\tSize(Segment)\t\t: %lld\n", pSegment->m_size);  // size of segment payload 

    mkvparser::Tracks* const pTracks = pSegment->GetTracks();

    unsigned long i = 0;
    const unsigned long j = pTracks->GetTracksCount();
    
    enum { VIDEO_TRACK = 1, AUDIO_TRACK = 2 };

    printf("\n\t\t\t   Track Info\n");
  
    while (i != j)
    {
        const Track* const pTrack = pTracks->GetTrackByIndex(i++);
        
        if (pTrack == NULL) 
            continue;

        const long long trackType_ = pTrack->GetType();
        unsigned long trackType = static_cast<unsigned long>(trackType_);
   
        unsigned long trackNumber = pTrack->GetNumber();
        const wchar_t* const pTrackName = utf8towcs(pTrack->GetNameAsUTF8());

        printf("\t\tTrack Type\t\t: %ld\n", trackType);
        printf("\t\tTrack Number\t\t: %ld\n", trackNumber);

        if (pTrackName == NULL)
            printf("\t\tTrack Name\t\t: NULL\n");
        else 
            printf("\t\tTrack Name\t\t: %ls \n", pTrackName);

        const char* const pCodecId = pTrack->GetCodecId();
    
        if (pCodecId == NULL) 
            printf("\t\tCodec Id\t\t: NULL\n");
        else 
            printf("\t\tCodec Id\t\t: %s\n", pCodecId);    

        const wchar_t* const pCodecName = utf8towcs(pTrack->GetCodecNameAsUTF8());
        
        if (pCodecName == NULL) 
            printf("\t\tCodec Name\t\t: NULL\n");
        else 
            printf("\t\tCodec Name\t\t: %ls\n", pCodecName);    

        if (trackType == VIDEO_TRACK)
        {
            const VideoTrack* const pVideoTrack = static_cast<const VideoTrack* const>(pTrack);
            long long width =  pVideoTrack->GetWidth();
            long long height = pVideoTrack->GetHeight();
            double rate = pVideoTrack->GetFrameRate();

            printf("\t\tVideo Width\t\t: %lld\n", width);
            printf("\t\tVideo Height\t\t: %lld\n", height);
            printf("\t\tVideo Rate\t\t: %f\n",rate);
        }
   
        if (trackType == AUDIO_TRACK)
        {
            const AudioTrack* const pAudioTrack = static_cast<const AudioTrack* const>(pTrack);
            long long channels =  pAudioTrack->GetChannels();
            long long bitDepth = pAudioTrack->GetBitDepth();
            double sampleRate = pAudioTrack->GetSamplingRate();

            printf("\t\tAudio Channels\t\t: %lld\n", channels);
            printf("\t\tAudio BitDepth\t\t: %lld\n", bitDepth);
            printf("\t\tAddio Sample Rate\t: %.3f\n", sampleRate);
        }
    }
      
    printf("\n\n\t\t\t   Cluster Info\n");
    const unsigned long clusterCount = pSegment->GetCount();

    printf("\t\tCluster Count\t: %ld\n\n", clusterCount);
         
    if (clusterCount == 0)
    {
        printf("\t\tSegment has no clusters.\n");
        delete pSegment;
        return -1;
    }

    mkvparser::Cluster* pCluster = pSegment->GetFirst();
    
    while ((pCluster != NULL) && !pCluster->EOS())
    {	
        const long long timeCode = pCluster->GetTimeCode();
        printf("\t\tCluster Time Code\t: %lld\n", timeCode);
 
        const long long time_ns = pCluster->GetTime();
        printf("\t\tCluster Time (ns)\t\t: %lld\n", time_ns); 
  
        const BlockEntry* pBlockEntry = pCluster->GetFirst(); 
  
        while ((pBlockEntry != NULL) && !pBlockEntry->EOS())
        {
            const Block* const pBlock  = pBlockEntry->GetBlock();
            const unsigned long trackNum = pBlock->GetTrackNumber(); 
            const long size = pBlock->GetSize();
            const long long time_ns = pBlock->GetTime(pCluster);

            printf("\t\t\tBlock\t\t:%s,%15ld,%s,%15lld\n",
                   (trackNum == VIDEO_TRACK) ? "V" : "A",
                   size, 
                   pBlock->IsKey() ? "I" : "P",
                   time_ns);

            pBlockEntry = pCluster->GetNext(pBlockEntry);
        }  

        pCluster = pSegment->GetNext(pCluster);
    }

    delete pSegment;
    return 0;

}
