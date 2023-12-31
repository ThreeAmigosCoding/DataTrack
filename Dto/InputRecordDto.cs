﻿using DataTrack.Model;

namespace DataTrack.Dto;

public class InputRecordDto
{
    public Guid Id { get; set; }
    public Guid InputId { get; set; }
    public string IoAddress { get; set; }
    public string Description  { get; set; }
    public double Value { get; set; }
    public string Driver {get; set;}
    public string DeviceName { get; set; }
    public bool ScanOn { get; set; }
    public bool IsDigital { get; set; }
    public string? Unit { get; set; }
    public DateTime RecordedAt { get; set; }

    public InputRecordDto()
    {
    }

    public InputRecordDto(Guid id, Guid inputId, string ioAddress, string description, double value, string driver, 
        string deviceName, bool scanOn, bool isDigital, string? unit, DateTime recordedAt)
    {
        Id = id;
        InputId = inputId;
        IoAddress = ioAddress;
        Description = description;
        Value = value;
        Driver = driver;
        DeviceName = deviceName;
        ScanOn = scanOn;
        IsDigital = isDigital;
        Unit = unit;
        RecordedAt = RecordedAt;
    }

    public InputRecordDto(AnalogInputRecord analogInputRecord, Device? device = null)
    {
        Id = analogInputRecord.Id;
        InputId = analogInputRecord.AnalogInput.Id;
        IoAddress = analogInputRecord.AnalogInput.IOAddress;
        Description = analogInputRecord.AnalogInput.Description;
        Value = analogInputRecord.Value;
        Driver = device == null ? "" : device.Driver;
        DeviceName = device == null ? "" : device.Name;
        ScanOn = analogInputRecord.AnalogInput.ScanOn;
        IsDigital = false;
        Unit = analogInputRecord.AnalogInput.Unit;
        RecordedAt = analogInputRecord.RecordedAt;
    }
    
    public InputRecordDto(DigitalInputRecord digitalInputRecord, Device? device = null)
    {
        Id = digitalInputRecord.Id;
        InputId = digitalInputRecord.DigitalInput.Id;
        IoAddress = digitalInputRecord.DigitalInput.IOAddress;
        Description = digitalInputRecord.DigitalInput.Description;
        Value = digitalInputRecord.Value;
        Driver = device == null ? "" : device.Driver;
        DeviceName = device == null ? "" : device.Name;
        ScanOn = digitalInputRecord.DigitalInput.ScanOn;
        IsDigital = true;
        Unit = "";
        RecordedAt = digitalInputRecord.RecordedAt;
    }
    
    
    
}